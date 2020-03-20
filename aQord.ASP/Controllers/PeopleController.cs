﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aQord.ASP.Models;
using aQord.ASP.ViewModels;

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

        
        public ActionResult Delete(int id)
        {
            var entity = _dbContext.PeopleDbSet.FirstOrDefault(p => p.Id == id);

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