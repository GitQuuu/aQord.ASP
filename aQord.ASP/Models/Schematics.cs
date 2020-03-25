using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aQord.ASP.Models
{
    public class Schematics
    {
        public int Id { get; set; }

        [Display(Name = "Arbejdets art")]
        public string TypeOfWork { get; set; }

        [Display(Name = "Tillidsmand")]
        public string StaffRepresentative { get; set; }

        [Display(Name = "Årstal")]
        public int Year { get; set; }

        [Display(Name = "Firma")]
        public string Firm { get; set; }

        [Display(Name = "Arbejdsplads")]
        public string WorkplaceAddress { get; set; }

        [Display(Name = "Akkord/Projekt nr.")]
        public string ProjectNumber { get; set; }

        [Display(Name = "Arbejds nr.")]
        public int CraftsmanId { get ; set; }

        [Display(Name = "Navn")]
        public string Name { get; set; }

        [Display(Name = "Uge nr.")]
        public int? WeekNumber { get; set; }

        [Display(Name = "Timer i akkord")]
        public double?[] AkkordHours { get; set; }

        [Display(Name = "Dagløns timer")]
        public double?[] NormalHours { get; set; }

        [Display(Name = "Skurpenge antal dage")]
        public double? ShelterRateAmountOfDays { get; set; }

        [Display(Name = "Kørepenge antal km.")]
        public double? MileageAllowanceAmountOfKm { get; set; }
        
    }
}