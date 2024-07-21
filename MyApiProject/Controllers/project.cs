using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using MyApiProject.Models;  // Add this line

namespace MyApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class projectController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<projectController> _logger;

        public projectController(IConfiguration configuration, ILogger<projectController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT Id, Name, Description FROM projects";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            MySqlDataReader myReader;
            try
            {
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {
                    mycon.Open();
                    using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        mycon.Close();
                    }
                }

                var result = DataTableToDictionaryList(table);

                _logger.LogInformation("Query executed successfully and data loaded.");
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the query.");
                return new JsonResult(new { message = "An error occurred while fetching data.", details = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Post(project project)
        {
            string query = @"INSERT INTO projects (Name,Description) VALUES (@Name,@Description)";

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            try
            {
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {
                    mycon.Open();
                    using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                    {
                        myCommand.Parameters.AddWithValue("@Name", project.Name);
                        myCommand.Parameters.AddWithValue("@Description", project.Description);

                        myCommand.ExecuteNonQuery();
                        mycon.Close();
                    }
                }

                _logger.LogInformation("New project record inserted successfully.");
                return new JsonResult(new { message = "project added successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while inserting the record.");
                return new JsonResult(new { message = "An error occurred while inserting data.", details = ex.Message });
            }
        }

        private List<Dictionary<string, object>> DataTableToDictionaryList(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            return list;
        }
    }
}
