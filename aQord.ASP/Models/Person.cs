using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aQord.ASP.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Display(Name="Fornavn")]
        public string FirstName { get; set; }

        [Display(Name = "Efternavn")]
        public string LastName { get; set; }

        [Display(Name = "Addresse")]
        public string Address { get; set; }

        [Display(Name = "By")]
        public string  City { get; set; }

        [Display(Name = "Post nummer")]
        [DataType(DataType.Text)]
        public int PostalCode { get; set; }

        [Display(Name = "Telefon nummer")]
        [DataType(DataType.Text)]
        public int CellphoneNo { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Arbejds stilling")]
        public string OccupationalStatus { get; set; }

        [Display(Name = "Løn i timen")]
        public decimal SalaryPrHour { get; set; }

        [Display(Name = "Ugentlig arbejdstid")]
        public double WeeklyWorkingHours { get; set; }

    }
}