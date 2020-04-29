using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aQord.ASP.Models;
using ClosedXML.Excel;
using Microsoft.Ajax.Utilities;

namespace aQord.ASP.Controllers
{
    public class SchematicsController : Controller
    {
        public ApplicationDbContext _dbContext;

        private IQueryable<Schematics> schematics;

        public SchematicsController()
        {
            _dbContext = new ApplicationDbContext();
        }

        // GET: Schematics
        public ActionResult Index(string searchString)
        {
            schematics = _dbContext.Schematics;

            if (!string.IsNullOrEmpty(searchString))
            {
                var words = searchString.Split(',');

                schematics = _dbContext.Schematics;

                foreach (var word in words)
                {
                    schematics = schematics.Where(s => word.Contains(s.TypeOfWork) ||
                                                       word.Contains(s.StaffRepresentative) ||
                                                       word.Contains(s.Year.ToString()) ||
                                                       word.Contains(s.Firm) ||
                                                       word.Contains(s.WorkplaceAddress.ToString()) ||
                                                       word.Contains(s.ProjectNumber.ToString()) ||
                                                       word.Contains(s.CraftsmanId.ToString()) ||
                                                       word.Contains(s.Name) ||
                                                       word.Contains(s.WeekNumber.ToString()) ||
                                                       word.Contains(s.HoursInAkkordData.ToString()) ||
                                                       word.Contains(s.NormalHoursData.ToString()) ||
                                                       word.Contains(s.ShelterRateAmountOfDays.ToString()) ||
                                                                                                                                                                 word.Contains(s.MileageAllowanceAmountOfKm.ToString()));
                }
            }

            return View(schematics);
        }

        //Action Function for dropdown  to populate other input fields in SchematicsForm
        [HttpPost]
        public ActionResult AutofillSchematicForm(string dropdownSelection)
        {
            var query = _dbContext.Schematics.FirstOrDefault(s => s.ProjectNumber == dropdownSelection);

            return Json(query);
        }

        //Action Function for dropdown  to populate other input fields in SchematicsForm for people
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

            var autoFill = _dbContext.People.AsEnumerable().Select(x => new { Id = x.FirstName, FullName = (x.Id + " " + x.FirstName + " " + x.LastName).ToString() });
            ViewData["FullName"] = new SelectList(autoFill, "Id", "FullName");

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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = _dbContext.Schematics.FirstOrDefault(s => s.Id == id);

            return View(entity);
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(Schematics schematics)
        {
            if (schematics == null)
            {
                return HttpNotFound();
            }


            var entity = _dbContext.Schematics.FirstOrDefault(s => s.Id == schematics.Id);
            _dbContext.Schematics.Remove(entity);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Schematics");
        }

        public ActionResult ExportToExcel(int id)
        {
            // use the  schematics variable locally instead of export from database or razorview

            IXLWorkbook workbook = new XLWorkbook("C:\\Users\\Quanv\\source\\repos\\aQord.ASP\\aQord.ASP\\Files\\UgeSkabelon.xlsx");
            IXLWorksheet pageTab = workbook.Worksheets.Worksheet(1);

            int row = 8;

            var selectedSchema = schematics.Where(s => s.Id == id);

            foreach (var data in selectedSchema)
            {
                pageTab.Cell($"T{3}").Value = data.ProjectNumber;
                pageTab.Cell($"B{1}").Value = data.TypeOfWork;
                pageTab.Cell($"J{1}").Value = data.StaffRepresentative;
                pageTab.Cell($"U{1}").Value = data.Year;
                pageTab.Cell($"B{3}").Value = data.Firm;
                pageTab.Cell($"I{3}").Value = data.WorkplaceAddress;
                pageTab.Cell($"B{6}").Value = data.WeekNumber;
            }

            workbook.SaveAs("C:\\Users\\Quanv\\source\\repos\\aQord.ASP\\aQord.ASP\\Files\\ExportToExcel");

            return View("Index");
        }
    }
}