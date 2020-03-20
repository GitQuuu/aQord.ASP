using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using aQord.ASP.Models;
using aQord.ASP.ViewModels;
using Microsoft.Ajax.Utilities;

namespace aQord.ASP.Controllers
{
    public class PeopleController : Controller
    {
        private ApplicationDbContext _dbContext;

        public PeopleController()
        {
            _dbContext = new ApplicationDbContext();
        }

        protected override void Dispose(bool dispose)
        {
            _dbContext.Dispose();
        }

        //Get data from database - my own solution
        //[HttpGet]
        //public ActionResult Index()
        //{
        //    List<Person> peopleFromDb = _dbContext.PeopleDbSet.ToList();

        //    return View(peopleFromDb);
        //}

        //Get data from database - from https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/introduction/adding-search
        [HttpGet]
        public ActionResult Index(string personCity, string searchString)
        {
            var cityList = new List<string>();

            var cityQry = from c in _dbContext.PeopleDbSet orderby c.City select c.City;

            cityList.AddRange(cityQry.Distinct());
            ViewBag.personCity = new SelectList(cityList);

            var person = from p in _dbContext.PeopleDbSet select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                person = _dbContext.PeopleDbSet.Where(s => s.FirstName.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(personCity))
            {
                person = _dbContext.PeopleDbSet.Where(x => x.City == personCity);
            }

            return View(person);
        }

        // direct user to the PeopleForm view
        public ActionResult New()
        {
            return View("PeopleForm");
        }

        // a method that saves user input from the PeopleForm.cshtml  
        [HttpPost]
        public ActionResult Save(Person person)
        {
            var craftsmanInDb = _dbContext.PeopleDbSet.Create();

            craftsmanInDb.FirstName = person.FirstName;
            craftsmanInDb.LastName = person.LastName;
            craftsmanInDb.Address = person.Address;
            craftsmanInDb.City = person.City;
            craftsmanInDb.PostalCode = person.PostalCode;
            craftsmanInDb.CellphoneNo = person.CellphoneNo;
            craftsmanInDb.Email = person.Email;
            craftsmanInDb.OccupationalStatus = person.OccupationalStatus;
            craftsmanInDb.SalaryPrHour = person.SalaryPrHour;
            craftsmanInDb.WeeklyWorkingHours = person.WeeklyWorkingHours;

            _dbContext.PeopleDbSet.Add(person);
            _dbContext.SaveChanges();

            return RedirectToAction("Index","People");
        }

        //Get: /Controller/Action/Id
        //add ? behind the int signature to allow nullable int, since int is never nullable pr. default
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                Person person = _dbContext.PeopleDbSet.Find(id);
                if (person == null)
                {
                    return HttpNotFound();
                }

                return View(person);

            }
           
        }

        //Post: /Controller/Action/Id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Person person)
        {
            if (person == null)
            {
                return HttpNotFound();
            }
            
            var entity = _dbContext.PeopleDbSet.FirstOrDefault(p => p.Id == person.Id);

            _dbContext.PeopleDbSet.Remove(entity);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "People");
        }

        // transfer text to Edit view
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = _dbContext.PeopleDbSet.FirstOrDefault(p => p.Id == id);

            return View(entity);
        }

        // Updating the entity
        [HttpPost]
        public ActionResult Update(Person person)
        {
            var entity = _dbContext.PeopleDbSet.FirstOrDefault(p => p.Id == person.Id);

            entity.FirstName = person.FirstName;
            entity.LastName = person.LastName;
            entity.Address = person.Address;
            entity.City = person.City;
            entity.PostalCode = person.PostalCode;
            entity.CellphoneNo = person.CellphoneNo;
            entity.Email = person.Email;
            entity.OccupationalStatus = person.OccupationalStatus;
            entity.SalaryPrHour = person.SalaryPrHour;
            entity.WeeklyWorkingHours = person.WeeklyWorkingHours;

            _dbContext.SaveChanges();

            return RedirectToAction("Index", "People");
        }
    }
}