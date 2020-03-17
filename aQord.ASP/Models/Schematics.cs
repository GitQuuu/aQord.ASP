using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aQord.ASP.Models
{
    public class Schematics
    {
        public int Id { get; set; }
        public string TypeOfWork { get; set; }
        public string StaffRepresentative { get; set; }
        public int Year { get; set; }
        public string Firm { get; set; }
        public string WorkplaceAddress { get; set; }
        public string ProjectNumber { get; set; }
        public int CraftsmanId { get; set; }
        public string Name { get; set; }
        public int WeekNumber { get; set; }
        public double[] AkkordHours { get; set; }
        public double[] NormalHours { get; set; }
        public double ShelterRateAmountOfDays { get; set; }
        public double MileageAllowanceAmountOfKm { get; set; }
        
    }
}