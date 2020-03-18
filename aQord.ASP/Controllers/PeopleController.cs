using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aQord.ASP.Models;

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

        [HttpGet]
        public ActionResult Index()
        {
            List<Person> peopleFromDb = _dbContext.PeopleDbSet.ToList();

            return View(peopleFromDb);
        }


        [HttpPost]
        public ActionResult New(Person person)
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

            return View();
        }

    }
}