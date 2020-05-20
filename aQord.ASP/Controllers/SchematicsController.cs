using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        private ObservableCollection<Schematics> ImportToCollection(long projectNumber)
        {
            ObservableCollection<Schematics> importFromDb = new ObservableCollection<Schematics>(_dbContext.Schematics.Where(s => s.ProjectNumber == projectNumber));

            return importFromDb;
        }

        [HttpGet]
        public ActionResult ExportToExcel(int id, long projectNumber = 0)
        {
            Schematics selected = _dbContext.Schematics.FirstOrDefault(s => s.Id == id);

            var selectionMatched = ImportToCollection(selected.ProjectNumber);

            projectNumber = selected.ProjectNumber;

            IXLWorkbook workbook = new XLWorkbook("https://qudevaspstorage.blob.core.windows.net/temp/UgeSkabelon.xlsx");
            IXLWorksheet pageTab = workbook.Worksheets.Worksheet(1);

            int row = 8;

            pageTab.Cell($"T{3}").Value = selected.ProjectNumber;
            pageTab.Cell($"B{1}").Value = selected.TypeOfWork;
            pageTab.Cell($"J{1}").Value = selected.StaffRepresentative;
            pageTab.Cell($"U{1}").Value = selected.Year;
            pageTab.Cell($"B{3}").Value = selected.Firm;
            pageTab.Cell($"I{3}").Value = selected.WorkplaceAddress;
            pageTab.Cell($"B{6}").Value = selected.WeekNumber;

            foreach (var schemas in selectionMatched)
            {
                if (schemas.ProjectNumber == projectNumber && schemas.WeekNumber == selected.WeekNumber)
                {
                    pageTab.Cell($"A{row}").Value = schemas.CraftsmanId;
                    pageTab.Cell($"B{row}").Value = schemas.Name;

                    // Hours in akkord row in excel
                    pageTab.Cell($"C{row}").Value = schemas.WeekNumber;
                    pageTab.Cell($"D{row}").Value = schemas.AkkordHours[0];
                    pageTab.Cell($"E{row}").Value = schemas.AkkordHours[1];
                    pageTab.Cell($"F{row}").Value = schemas.AkkordHours[2];
                    pageTab.Cell($"G{row}").Value = schemas.AkkordHours[3];
                    pageTab.Cell($"H{row}").Value = schemas.AkkordHours[4];
                    pageTab.Cell($"I{row}").Value = schemas.AkkordHours[5];
                    pageTab.Cell($"J{row}").Value = schemas.AkkordHours[6];


                    // Hours in normal row in excel schemas
                    pageTab.Cell($"M{row}").Value = schemas.NormalHours[0];
                    pageTab.Cell($"N{row}").Value = schemas.NormalHours[1];
                    pageTab.Cell($"O{row}").Value = schemas.NormalHours[2];
                    pageTab.Cell($"P{row}").Value = schemas.NormalHours[3];
                    pageTab.Cell($"Q{row}").Value = schemas.NormalHours[4];
                    pageTab.Cell($"R{row}").Value = schemas.NormalHours[5];
                    pageTab.Cell($"S{row}").Value = schemas.NormalHours[6];

                    row++;
                }

            }

            // How to export to excel without saving file on the server
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                stream.Flush();

                return new FileContentResult(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = $"ProjectNummer_{selected.ProjectNumber}-Uge_{selected.WeekNumber}.xlsx"
                };
            }
        }

    }
}