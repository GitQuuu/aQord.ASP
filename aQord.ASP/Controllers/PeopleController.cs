﻿using System;
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

        //Get data from database
        [HttpGet]
        public ActionResult Index(string searchString, string filterByCity)
        {
            IQueryable<Person> people = _dbContext.People;

            //var people =  from p in _dbContext.People
            //              select p;
            

            var cityList = new List<string>();

            var cityQry =    from c in _dbContext.People 
                                                orderby c.City 
                                                select c.City;



            cityList.AddRange(cityQry.Distinct());
            ViewBag.filterByCity = new SelectList(cityList);


            //For the searchbox in view
            if (!string.IsNullOrEmpty(searchString))
            {
                 var searchWords = searchString.Split(',');

                people = _dbContext.People.Where(p => searchWords.Contains(p.FirstName) || 
                                                      searchWords.Contains(p.LastName) ||
                                                      searchWords.Contains(p.Address) ||
                                                      searchWords.Contains(p.City) ||
                                                      searchWords.Contains(p.PostalCode.ToString()) ||
                                                      searchWords.Contains(p.CellphoneNo.ToString()) ||
                                                      searchWords.Contains(p.Email) ||
                                                      searchWords.Contains(p.OccupationalStatus) ||
                                                      searchWords.Contains(p.SalaryPrHour.ToString()) ||
                                                      searchWords.Contains(p.WeeklyWorkingHours.ToString()) 
                                                      );

            }

            //For the dropdown in the view
            if (!string.IsNullOrEmpty(filterByCity))
            {
                people = _dbContext.People.Where(p => p.City == filterByCity);
            }

            return View(people);
        }

        ////Get data from database - from https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/introduction/adding-search


        // direct user to the PeopleForm view
        public ActionResult New()
        {
            return View("PeopleForm");
        }

        // a method that saves user input from the PeopleForm.cshtml  
        [HttpPost]
        public ActionResult Save(Person person)
        {
            var craftsmanInDb = _dbContext.People.Create();

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

            _dbContext.People.Add(person);
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
                Person person = _dbContext.People.Find(id);
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
            
            var entity = _dbContext.People.FirstOrDefault(p => p.Id == person.Id);

            _dbContext.People.Remove(entity);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "People");
        }

        // transfer text to Edit view
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = _dbContext.People.FirstOrDefault(p => p.Id == id);

            return View(entity);
        }

        // Updating the entity
        [HttpPost]
        public ActionResult Update(Person person)
        {
            var entity = _dbContext.People.FirstOrDefault(p => p.Id == person.Id);

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