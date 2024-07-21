using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MyApiProject.Models
{
    public class PerformanceReview
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string ReviewDate { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
    }
}
