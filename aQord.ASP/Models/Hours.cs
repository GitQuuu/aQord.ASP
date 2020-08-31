using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aQord.ASP.Models
{
    public class Hours
    {
        public int HoursId { get; set; }
        public double AkkordHours { get; set; }
        public double NormalHours { get; set; }
        public DayOfWeek Day { get; set; }
        // By adding this properties EF will create a columnn that has a relation to Schematics Table in the database
        public Schematics Schematics { get; set; }

        // Convention 2 - https://www.entityframeworktutorial.net/code-first/configure-one-to-many-relationship-in-code-first.aspx
        /*In the above example, the Grade entity includes a collection navigation property of type ICollection<Schematics>.
         This results in a one-to-many relationship between the Schematic and Hours entities. */
     
    }
}