using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using MyApiProject.Models;

namespace MyApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerformanceReviewController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PerformanceReviewController> _logger;

        public PerformanceReviewController(IConfiguration configuration, ILogger<PerformanceReviewController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT Id, EmployeeId, ReviewDate, Comments, Rating FROM performancereviews";

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
        public JsonResult Post([FromBody] PerformanceReview performanceReview)
        {
            string query = @"INSERT INTO performancereviews (EmployeeId, ReviewDate, Comments, Rating) 
                             VALUES (@EmployeeId, @ReviewDate, @Comments, @Rating)";

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            try
            {
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {
                    mycon.Open();
                    using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                    {
                        myCommand.Parameters.AddWithValue("@EmployeeId", performanceReview.EmployeeId);
                        myCommand.Parameters.AddWithValue("@ReviewDate", performanceReview.ReviewDate);
                        myCommand.Parameters.AddWithValue("@Comments", performanceReview.Comments);
                        myCommand.Parameters.AddWithValue("@Rating", performanceReview.Rating);

                        myCommand.ExecuteNonQuery();
                        mycon.Close();
                    }
                }

                _logger.LogInformation("New performance review record inserted successfully.");
                return new JsonResult(new { message = "Performance review added successfully." });
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
