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

        //Action Function for dropdown  to populate other textbos in SchematicsForm
        [HttpPost]
        public ActionResult AutofillSchematicForm(string dropdownSelection)
        {
            var query = _dbContext.Schematics.FirstOrDefault(s => s.ProjectNumber == dropdownSelection);

            return Json(query);
        }

        [HttpPost]
        public ActionResult AutofillSchematicFormPeople(string dropdownSelection)
        {
            var query = _dbContext.People.FirstOrDefault(s => s.FirstName == dropdownSelection);

            return Json(query);
        }



        public ActionResult New()
        {
            ViewData["ProjectNumbers"] = new SelectList(_dbContext.Schematics.GroupBy(a => a.ProjectNumber).Select(g => g.FirstOrDefault()), "ProjectNumber", "ProjectNumber");
            //ViewData["Person"] = new SelectList(_dbContext.People, "FirstName", "FirstName");

            var medlist = _dbContext.People.AsEnumerable().Select(x => new { Id = x.FirstName, FullName = (x.Id + " " + x.FirstName + " " + x.LastName).ToString() });
            ViewData["FullName"] = new SelectList(medlist,"Id","FullName");
          
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

        // Get
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            else
            {
                var schematic = _dbContext.Schematics.Find(id);

                if (schematic == null)
                {
                    return HttpNotFound();
                }
                

                return View(schematic);

            }

        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(Schematics schematics)
        {
            if (schematics == null)
            {
                return HttpNotFound();
            }


            var entity = _dbContext.Schematics.FirstOrDefault(s=>s.Id == schematics.Id);
            _dbContext.Schematics.Remove(entity);
            _dbContext.SaveChanges();

            return RedirectToAction("Index","Schematics");
        }
    }
}