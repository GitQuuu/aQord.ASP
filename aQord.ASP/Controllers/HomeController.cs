using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aQord.ASP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            // added these code for testing if program will catch NullReferenceException https://stackoverflow.com/questions/779091/what-does-object-reference-not-set-to-an-instance-of-an-object-mean
            
            /*var exampleClass = new ExampleClass();
            var returnedClass = exampleClass.ExampleMethod();
            returnedClass.AnotherExampleMethod(); //NullReferenceException here.*/

            return View();
        }

        class ExampleClass
        {
            public ReturnedClass ExampleMethod()
            {
                return null;
            }
        }

        class ReturnedClass
        {
            public void AnotherExampleMethod()
            {
            }
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Kontakt side.";

            return View();
        }
    }
}