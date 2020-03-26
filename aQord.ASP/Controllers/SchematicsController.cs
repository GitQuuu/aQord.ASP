using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aQord.ASP.Models;

namespace aQord.ASP.Controllers
{
    public class SchematicsController : Controller
    {
        public ApplicationDbContext _dbContext;

        public SchematicsController()
        {
            _dbContext = new ApplicationDbContext();
        }

        // GET: Schematics
        public ActionResult Index()
        {
            return View(_dbContext.Schematics);
        }

        public ActionResult New()
        {
            return View("SchematicsForm");
        }

        public ActionResult Save(Schematics schematic)
        {
            var schematicsToDb = _dbContext.Schematics.Create();

            schematicsToDb.Id = schematic.Id;
            schematicsToDb.TypeOfWork = schematic.TypeOfWork;
            schematicsToDb.StaffRepresentative = schematic.StaffRepresentative;
            schematicsToDb.Year = schematic.Year;
            schematicsToDb.Firm = schematic.Firm;
            schematicsToDb.WorkplaceAddress = schematic.WorkplaceAddress;
            schematicsToDb.ProjectNumber = schematic.ProjectNumber;
            schematicsToDb.CraftsmanId = schematic.CraftsmanId;
            schematicsToDb.Name = schematic.Name;
            schematicsToDb.WeekNumber = schematic.WeekNumber;
            schematicsToDb.AkkordHours = schematic.AkkordHours;
            schematicsToDb.NormalHours = schematic.NormalHours;

            _dbContext.Schematics.Add(schematic);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Schematics");
        }
    }
}