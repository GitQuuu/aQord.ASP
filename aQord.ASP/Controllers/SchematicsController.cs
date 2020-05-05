using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aQord.ASP.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.Ajax.Utilities;

namespace aQord.ASP.Controllers
{
    public class SchematicsController : Controller
    {
        public ApplicationDbContext _dbContext;

        public SchematicsController()
        {
            _dbContext = new ApplicationDbContext();
        }

        private IQueryable<Schematics> schematics;

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
            var query = _dbContext.Schematics.FirstOrDefault(s => s.ProjectNumber.ToString() == dropdownSelection);

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

        [HttpPost]
        public ActionResult Update(Schematics schematics)
        {
            var entity = _dbContext.Schematics.FirstOrDefault(p => p.Id == schematics.Id);

            entity.TypeOfWork = schematics.TypeOfWork;
            entity.StaffRepresentative = schematics.StaffRepresentative;
            entity.Year = schematics.Year;
            entity.Firm = schematics.Firm;
            entity.WorkplaceAddress = schematics.WorkplaceAddress;
            entity.ProjectNumber = schematics.ProjectNumber;
            entity.CraftsmanId = schematics.CraftsmanId;
            entity.Name = schematics.Name;
            entity.WeekNumber = schematics.WeekNumber;
            entity.AkkordHours = schematics.AkkordHours;
            entity.NormalHours = schematics.NormalHours;
            entity.ShelterRateAmountOfDays = schematics.ShelterRateAmountOfDays;
            entity.MileageAllowanceAmountOfKm = schematics.MileageAllowanceAmountOfKm;

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

        public ActionResult ExportToExcel(int projectNumber)
        {
            var selectedSchema = _dbContext.Schematics.FirstOrDefault(s => s.ProjectNumber == projectNumber);

            string fileName = $"C:\\Users\\Quanv\\source\\repos\\aQord.ASP\\aQord.ASP\\Files\\ExportToExcel\\ProjectNummer_{selectedSchema.ProjectNumber}-Uge_{selectedSchema.WeekNumber}.xlsx";

            // use the  schematics variable locally instead of export from database or razorview

            IXLWorkbook workbook = new XLWorkbook("C:\\Users\\Quanv\\source\\repos\\aQord.ASP\\aQord.ASP\\Files\\UgeSkabelon.xlsx");
            IXLWorksheet pageTab = workbook.Worksheets.Worksheet(1);

            int row = 8;

            foreach (var entity in _dbContext.Schematics)
            {
                
            }

            pageTab.Cell($"A{row}").Value = selectedSchema.CraftsmanId;
            pageTab.Cell($"B{row}").Value = selectedSchema.Name;
           
                pageTab.Cell($"T{3}").Value = selectedSchema.ProjectNumber;
                pageTab.Cell($"B{1}").Value = selectedSchema.TypeOfWork;
                pageTab.Cell($"J{1}").Value = selectedSchema.StaffRepresentative;
                pageTab.Cell($"U{1}").Value = selectedSchema.Year;
                pageTab.Cell($"B{3}").Value = selectedSchema.Firm;
                pageTab.Cell($"I{3}").Value = selectedSchema.WorkplaceAddress;
                pageTab.Cell($"B{6}").Value = selectedSchema.WeekNumber;

            // Hours in akkord row in excel
            pageTab.Cell($"C{row}").Value = selectedSchema.WeekNumber;
            pageTab.Cell($"D{row}").Value = selectedSchema.AkkordHours[0];
            pageTab.Cell($"E{row}").Value = selectedSchema.AkkordHours[1];
            pageTab.Cell($"F{row}").Value = selectedSchema.AkkordHours[2];
            pageTab.Cell($"G{row}").Value = selectedSchema.AkkordHours[3];
            pageTab.Cell($"H{row}").Value = selectedSchema.AkkordHours[4];
            pageTab.Cell($"I{row}").Value = selectedSchema.AkkordHours[5];
            pageTab.Cell($"J{row}").Value = selectedSchema.AkkordHours[6];

            
           
            // Hours in normal row in excel selectedSchema
            pageTab.Cell($"M{row}").Value = selectedSchema.NormalHours[0];
            pageTab.Cell($"N{row}").Value = selectedSchema.NormalHours[1];
            pageTab.Cell($"O{row}").Value = selectedSchema.NormalHours[2];
            pageTab.Cell($"P{row}").Value = selectedSchema.NormalHours[3];
            pageTab.Cell($"Q{row}").Value = selectedSchema.NormalHours[4];
            pageTab.Cell($"R{row}").Value = selectedSchema.NormalHours[5];
            pageTab.Cell($"S{row}").Value = selectedSchema.NormalHours[6];

            

            row++;

            workbook.SaveAs(fileName);

            return RedirectToAction("Index", "Schematics");
        }
    }
}