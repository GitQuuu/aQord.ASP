using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml;

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

        [Display(Name = "Akkord/Projekt")]
        public long ProjectNumber { get; set; }

        [Display(Name = "Arbejds nr.")]
        public int CraftsmanId { get ; set; }

        [Display(Name = "Navn")]
        public string Name { get; set; }

        [Display(Name = "Uge")]
        public int? WeekNumber { get; set; }

        [NotMapped]
        [Display(Name = "Akkord timer")]
        public List<double> AkkordHours
        {
            get
            {
                if (!string.IsNullOrEmpty(HoursInAkkordData))
                {
                    return Array.ConvertAll(HoursInAkkordData.Split(), double.Parse).ToList();
                }

                return new List<double>(7);
            }
            set
            {
                var _data = value;
                HoursInAkkordData = String.Join(" ", _data.Select(d => d.ToString()).ToArray());
            }
        }

        [Display(Name = "Akkord timer")]
        public string HoursInAkkordData { get; set; }

        [NotMapped]
        [Display(Name = "Dagløns timer")]
        public List<double> NormalHours {
            get
            {
                if (!string.IsNullOrEmpty(NormalHoursData))
                {
                    return Array.ConvertAll(NormalHoursData.Split(), double.Parse).ToList();
                }
                else
                {
                   return new List<double>(7);
                }
            }
            set
            {
                var _data = value;
                NormalHoursData = String.Join(" ", _data.Select(d => d.ToString()).ToArray());
            }
        }

        [Display(Name = "Dagløns timer")]
        public string NormalHoursData { get; set; }

        [Display(Name = "Skurpenge antal dage")]
        public double? ShelterRateAmountOfDays { get; set; }

        [Display(Name = "Kørepenge antal km.")]
        public double? MileageAllowanceAmountOfKm { get; set; }
        
    }
}