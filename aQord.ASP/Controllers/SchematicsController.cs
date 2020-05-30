using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using aQord.ASP.Models;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Ajax.Utilities;
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


        // GET: Schematics
        [Authorize]
        public ActionResult Index(string searchString)
        {

        IQueryable<Schematics> schematics = _dbContext.Schematics;
        
            if (!string.IsNullOrEmpty(searchString))
            {
                var words = searchString.Split(',');

                schematics = _dbContext.Schematics;

                foreach (var word in words)
                {
                    schematics = schematics.Where(s => word.Equals(s.TypeOfWork) ||
                                                       word.Equals(s.StaffRepresentative) ||
                                                       word.Equals(s.Year.ToString()) ||
                                                       word.Equals(s.Firm) ||
                                                       word.Equals(s.WorkplaceAddress.ToString()) ||
                                                       word.Equals(s.ProjectNumber.ToString()) ||
                                                       word.Equals(s.CraftsmanId.ToString()) ||
                                                       word.Equals(s.Name) ||
                                                       word.Equals(s.WeekNumber.ToString()) ||
                                                       word.Equals(s.HoursInAkkordData.ToString()) ||
                                                       word.Equals(s.NormalHoursData.ToString()) ||
                                                       word.Equals(s.ShelterRateAmountOfDays.ToString()) ||
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


                return View(schematic);

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
            // Authenticate and create a client to retrieve keys&Secrets from the KeyVault https://docs.microsoft.com/en-us/azure/key-vault/secrets/quick-create-net
            var KeyVaultName = ConfigurationManager.AppSettings["KeyVaultName"];
            var kvUri = "https://" + KeyVaultName + ".vault.azure.net";
            var clientVault = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            var KeyVaultSecret = clientVault.GetSecret("AzureBlobStorage").Value;

            try
            {
                // How to use file from Azure blob storage https://stackoverflow.com/questions/45442818/how-to-download-files-from-azure-blob-storage-with-a-download-link

                var filename = "UgeSkabelon.xlsx";
                var storageAccount = CloudStorageAccount.Parse($"{KeyVaultSecret.Value}");
                var blobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer container = blobClient.GetContainerReference("temp");
                CloudBlockBlob blob = container.GetBlockBlobReference(filename);

                Stream blobStream = blob.OpenRead();

                return blobStream;

                //return File(blobStream, blob.Properties.ContentType, filename);

            }
            catch (Exception)
            {
                //download failed 
                //handle exception
                throw;
            }
        }

        [HttpGet]
        public ActionResult ExportToExcel(int id, long projectNumber = 0)
        {
            Schematics selected = _dbContext.Schematics.FirstOrDefault(s => s.Id == id);

            var selectionMatched = ImportToCollection(selected.ProjectNumber);

            projectNumber = selected.ProjectNumber;

            var getDownloadBlobFile = DownloadBlobFile();


            IXLWorkbook workbook = new XLWorkbook(getDownloadBlobFile); // since XLWorkBook use a stream our DownloadBlobFile returns a stream
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