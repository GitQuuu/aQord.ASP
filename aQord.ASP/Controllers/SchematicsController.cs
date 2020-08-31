using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aQord.ASP.Models;
using aQord.ASP.Services;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ClosedXML;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace aQord.ASP.Controllers
{
    public class SchematicsController : Controller
    {
        public ApplicationDbContext _dbContext;

        public SchematicsController()
        {
            _dbContext = new ApplicationDbContext();
        }

        public void ViewDatas()
        {
            // Creating ViewData for each Dropdowns in the Schematics Index.cshtml - https://stackoverflow.com/questions/12090937/populating-a-dropdown-from-viewdata
            ViewData["TypeOfWork"] = new SelectList(_dbContext.Schematics.GroupBy(a => a.TypeOfWork).Select(g => g.FirstOrDefault()), "TypeOfWork", "TypeOfWork");
            ViewData["StaffRepresentative"] = new SelectList(_dbContext.Schematics.GroupBy(a => a.StaffRepresentative).Select(g => g.FirstOrDefault()), "StaffRepresentative", "StaffRepresentative");
            ViewData["Year"] = new SelectList(_dbContext.Schematics.GroupBy(a => a.Year).Select(g => g.FirstOrDefault()), "Year", "Year");
            ViewData["Firm"] = new SelectList(_dbContext.Schematics.GroupBy(a => a.Firm).Select(g => g.FirstOrDefault()), "Firm", "Firm");
            ViewData["WorkplaceAddress"] = new SelectList(_dbContext.Schematics.GroupBy(a => a.WorkplaceAddress).Select(g => g.FirstOrDefault()), "WorkplaceAddress", "WorkplaceAddress");
            ViewData["ProjectNumbers"] = new SelectList(_dbContext.Schematics.GroupBy(a => a.ProjectNumber).Select(g => g.FirstOrDefault()), "ProjectNumber", "ProjectNumber");
            ViewData["CraftsmanId"] = new SelectList(_dbContext.Schematics.GroupBy(a => a.CraftsmanId).Select(g => g.FirstOrDefault()), "CraftsmanId", "CraftsmanId");
            ViewData["Name"] = new SelectList(_dbContext.Schematics.GroupBy(a => a.Name).Select(g => g.FirstOrDefault()), "Name", "Name");
            ViewData["WeekNumber"] = new SelectList(_dbContext.Schematics.GroupBy(a => a.WeekNumber).Select(g => g.FirstOrDefault()), "WeekNumber", "WeekNumber");
            ViewData["AkkordHours"] = new SelectList(_dbContext.Schematics.GroupBy(a => a.AkkordHours).Select(g => g.FirstOrDefault()), "AkkordHours", "AkkordHours");
            ViewData["NormalHours"] = new SelectList(_dbContext.Schematics.GroupBy(a => a.NormalHours).Select(g => g.FirstOrDefault()), "NormalHours", "NormalHours");
            ViewData["ShelterRateAmountOfDays"] = new SelectList(_dbContext.Schematics.GroupBy(a => a.ShelterRateAmountOfDays).Select(g => g.FirstOrDefault()), "ShelterRateAmountOfDays", "ShelterRateAmountOfDays");
            ViewData["MileAgeAllowanceAmountOfKm"] = new SelectList(_dbContext.Schematics.GroupBy(a => a.MileageAllowanceAmountOfKm).Select(g => g.FirstOrDefault()), "MileAgeAllowanceAmountOfKm", "MileAgeAllowanceAmountOfKm");

            // combining multiple information in one dropdown
            var combineForDropdownPerson = _dbContext.People.AsEnumerable().Select(x => new { Id = x.FirstName, FullName = (x.Id + ": " + x.FirstName + " " + x.LastName).ToString() });
            ViewData["FullName"] = new SelectList(combineForDropdownPerson, "Id", "FullName");

            var combineForDropdownProject = _dbContext.Schematics.AsEnumerable().Select(x => new { ProjectNumber = x.ProjectNumber, TypeOfWork = (x.ProjectNumber + ": " + x.TypeOfWork + ", " + x.Firm).ToString() }).Distinct();
            ViewData["ProjectNumberDescription"] = new SelectList(combineForDropdownProject, "ProjectNumber", "TypeOfWork");

        }

        // GET: Schematics
        [Authorize]
        public ActionResult Index(string typeOfWork, string staffRepresentative, int? year, string firm, string workplaceAddress, long? projectNumber, int? craftsmanId, string name, int? weekNumber)
        {
            ViewDatas();

            // if user logon is in Admin role show everything from table, if not show only table data related to logon user name.
            var schematics = HttpContext.User.IsInRole("Admin") ? _dbContext.Schematics : _dbContext.Schematics.Where(s => s.CreatedBy == HttpContext.User.Identity.Name);

            if (!string.IsNullOrEmpty(typeOfWork))
            {
                schematics = schematics.Where(x => x.TypeOfWork.Equals(typeOfWork));
            }


            if (!string.IsNullOrEmpty(staffRepresentative))
            {
                schematics = schematics.Where(x => x.StaffRepresentative.Equals(staffRepresentative));
            }

            if (!string.IsNullOrEmpty(year.ToString()))
            {
                schematics = schematics.Where(x => x.Year == year);
            }

            if (!string.IsNullOrEmpty(firm))
            {
                schematics = schematics.Where(x => x.Firm.Equals(firm));
            }

            if (!string.IsNullOrEmpty(workplaceAddress))
            {
                schematics = schematics.Where(x => x.WorkplaceAddress.Equals(workplaceAddress));
            }

            if (!string.IsNullOrEmpty(Convert.ToString(projectNumber)))
            {
                schematics = schematics.Where(x => x.ProjectNumber == projectNumber);
            }

            //if (!string.IsNullOrEmpty(craftsmanId.ToString()))
            //{
            //    schematics = schematics.Where(x => x.CraftsmanId == craftsmanId);
            //}

            if (!string.IsNullOrEmpty(name))
            {
                schematics = schematics.Where(x => x.Name.Equals(name));
            }

            if (!string.IsNullOrEmpty(weekNumber.ToString()))
            {
                schematics = schematics.Where(x => x.WeekNumber == weekNumber);
            }


            return View(schematics);
        }

        //Action Function for dropdown  to populate other input fields in SchematicsForm
        [HttpPost]
        public ActionResult AutoFillSchematicForm(string dropdownSelection)
        {
            var query = _dbContext.Schematics.FirstOrDefault(s => s.ProjectNumber.ToString() == dropdownSelection);

            return Json(query);
        }

        //Action Function for dropdown  to populate other input fields in SchematicsForm for people
        [HttpPost]
        public ActionResult AutoFillSchematicFormPeople(string dropdownSelection)
        {
            var query = _dbContext.People.FirstOrDefault(s => s.FirstName == dropdownSelection);

            return Json(query);
        }



        public ActionResult New()
        {
            ViewDatas();

            return PartialView("SchematicsForm");
        }

        /// <summary>
        /// Save data to a collection, from a view  
        /// </summary>
        /// <param name="schematic"></param>
        /// <returns>ICollection</returns>
        public ICollection<Hours> SaveHoursToICollection(Schematics schematic)
        {
            ICollection<Hours> hoursICollection = schematic.HoursICollection;

            hoursICollection.Add(new Hours
                {

                    AkkordHours = schematic.AkkordHours[0],
                    NormalHours = schematic.NormalHours[0],
                    Day = "Mon",
                }
            );

            hoursICollection.Add(new Hours
                {

                    AkkordHours = schematic.AkkordHours[1],
                    NormalHours = schematic.NormalHours[1],
                    Day = "Tue",
                }
            );

            hoursICollection.Add(new Hours
                {

                    AkkordHours = schematic.AkkordHours[2],
                    NormalHours = schematic.NormalHours[2],
                    Day = "Wed",
                }
            );

            hoursICollection.Add(new Hours
                {

                    AkkordHours = schematic.AkkordHours[3],
                    NormalHours = schematic.NormalHours[3],
                    Day = "Thu",
                }
            );

            hoursICollection.Add(new Hours
                {

                    AkkordHours = schematic.AkkordHours[4],
                    NormalHours = schematic.NormalHours[4],
                    Day = "Fri",
                }
            );

            hoursICollection.Add(new Hours
                {

                    AkkordHours = schematic.AkkordHours[5],
                    NormalHours = schematic.NormalHours[5],
                    Day = "Sat",
                }
            );

            hoursICollection.Add(new Hours
                {

                    AkkordHours = schematic.AkkordHours[6],
                    NormalHours = schematic.NormalHours[6],
                    Day = "Sun",
                }
            );

            return hoursICollection;
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


            schematicsToDb.HoursInAkkordData = schematic.HoursInAkkordData;
            schematicsToDb.NormalHoursData = schematic.NormalHoursData;

            // Save current user directly to the database when creating a new schematic and click on save actionresult - https://stackoverflow.com/questions/263486/how-to-get-the-current-user-in-asp-net-mvc
            schematic.CreatedBy = HttpContext.User.Identity.Name;

            // Using a method that handles data input from view , to save to HoursICollection in model
            schematic.HoursICollection = SaveHoursToICollection(schematic);
            
            _dbContext.Schematics.Add(schematic);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Schematics");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = _dbContext.Schematics.FirstOrDefault(s => s.Id == id);

            return PartialView(entity);
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
            entity.HoursInAkkordData = schematics.HoursInAkkordData;
            entity.NormalHoursData = schematics.NormalHoursData;
            entity.ShelterRateAmountOfDays = schematics.ShelterRateAmountOfDays;
            entity.MileageAllowanceAmountOfKm = schematics.MileageAllowanceAmountOfKm;

            if (entity.MySignature == null)
            {
                entity.MySignature = schematics.MySignature;
            }

            if (entity.EmployerSignature == null)
            {
                entity.EmployerSignature = schematics.EmployerSignature;
            }



            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Schematics");
        }

        // Get
        [Authorize(Roles = "Admin")]
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


                return PartialView(schematic);

            }

        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
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

        // returntype is stream because our ExportToExcel methods requireds a stream to get the file from azure
        public Stream DownloadBlobFile()
        {
            try
            {

                var filename = "UgeSkabelon.xlsx";
                var storageAccount = CloudStorageAccount.Parse(KeyVaultService.KeyVaultSecret("AzureBlobStorage", KeyVaultService.AuthenticateCreateClient()).Value);
                var blobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer container = blobClient.GetContainerReference("temp");
                CloudBlockBlob blob = container.GetBlockBlobReference(filename);

                Stream blobStream = blob.OpenRead();
                return blobStream;

                // How to use file from Azure blob storage https://stackoverflow.com/questions/45442818/how-to-download-files-from-azure-blob-storage-with-a-download-link

            }
            catch (Exception)
            {
                //download failed 
                //handle exception
                throw;
            }
        }

        [HttpGet]
        public ActionResult ExportToExcel(int id)
        {
            // Select a schematic by ID to export
            Schematics selected = _dbContext.Schematics.FirstOrDefault(s => s.Id == id);

            // Find all schematics that have the same ProjectNumber
            ObservableCollection<Schematics> selectionMatched = ImportToCollection(selected.ProjectNumber);

            var getDownloadBlobFile = DownloadBlobFile();

            // https://github.com/ClosedXML/ClosedXML/wiki
            IXLWorkbook workbook = new XLWorkbook(getDownloadBlobFile); // since XLWorkBook use a stream our DownloadBlobFile returns a stream
            IXLWorksheet pageTab = workbook.Worksheets.Worksheet(1);

            int row = 8;

            // export data from the selected to excel
            pageTab.Cell($"T{3}").Value = selected.ProjectNumber;
            pageTab.Cell($"B{1}").Value = selected.TypeOfWork;
            pageTab.Cell($"J{1}").Value = selected.StaffRepresentative;
            pageTab.Cell($"U{1}").Value = selected.Year;
            pageTab.Cell($"B{3}").Value = selected.Firm;
            pageTab.Cell($"I{3}").Value = selected.WorkplaceAddress;


            if (selected.EmployerSignature != null)
            {
                // Exporting byte[] to an image to use in excel https://forums.asp.net/t/1570944.aspx?Exporting+image+byte+stream+to+Excel
                var employerSignature = pageTab.Pictures.Add(new MemoryStream(selected.EmployerSignature));
                // How to insert an image image  https://github.com/ClosedXML/ClosedXML/wiki/How-can-I-insert-an-image
                employerSignature.MoveTo(pageTab.Cell($"H{22}")).Scale(.3);
            }


            if (selected.MySignature != null)
            {
                var mySignature = pageTab.Pictures.Add(new MemoryStream(selected.MySignature));
                mySignature.MoveTo(pageTab.Cell($"H{24}")).Scale(.3);

            }



            foreach (var schemas in selectionMatched)
            { // base on selectionMatched only export data from db to excel with schemas that contains Projectnumber and weeknumber
                if (schemas.ProjectNumber == selected.ProjectNumber && schemas.WeekNumber == selected.WeekNumber)
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

            // How to export to excel without saving file on the server https://stackoverflow.com/questions/22296136/download-file-with-closedxml/22298678 scroll down to Phils solutions
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

        [HttpGet]
        public ActionResult Details(int id)
        {
            var entity = _dbContext.Schematics.Find(id);

            if (entity == null)
            {
                return HttpNotFound();
            }

            return PartialView("Details", entity);
        }
    }
}