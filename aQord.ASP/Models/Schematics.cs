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
        [DataType(DataType.Text)]
        public int Year { get; set; }

        [Display(Name = "Firma")]
        public string Firm { get; set; }

        [Display(Name = "Arbejdsplads")]
        public string WorkplaceAddress { get; set; }

        [Display(Name = "Akkord/Projekt nr.")]
        public Int64 ProjectNumber { get; set; }

        [Display(Name = "Arbejds nr.")]
        [DataType(DataType.Text)]
        public int CraftsmanId { get ; set; }

        [Display(Name = "Navn")]
        public string Name { get; set; }

        [Display(Name = "Uge nr.")]
        [DataType(DataType.Text)]
        public int? WeekNumber { get; set; }

        [NotMapped]
        [Display(Name = "Timer i akkord")]
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

        [Display(Name = "Timer i akkord")]
        public string HoursInAkkordData { get; set; }

        [NotMapped]
        [Display(Name = "Dagløns timer")]
        public double[] NormalHours {
            get
            {
                if (!string.IsNullOrEmpty(NormalHoursData))
                {
                    return Array.ConvertAll(NormalHoursData.Split(','), double.Parse);
                }
                else
                {
                   return new double[7]; 
                }
            }
            set
            {
                var _data = value;
                NormalHoursData = String.Join(",", _data.Select(d => d.ToString()).ToArray());
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