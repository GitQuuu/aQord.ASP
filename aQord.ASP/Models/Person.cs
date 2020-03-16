using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aQord.ASP.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string  City { get; set; }
        public int PostalCode { get; set; }
        public int CellphoneNo { get; set; }
        public string Email { get; set; }
        public string OccupationalStatus { get; set; }
        public decimal SalaryPrHour { get; set; }
        public double WeeklyWorkingHours { get; set; }

    }
}