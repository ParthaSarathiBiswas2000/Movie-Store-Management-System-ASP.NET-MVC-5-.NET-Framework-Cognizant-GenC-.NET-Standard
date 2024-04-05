using signup_with_login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace signup_with_login.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        SignuploginEntities1Entities db = new SignuploginEntities1Entities();

        [Authorize]
        public ActionResult Index()
        {
            var movies = db.movies.ToList();
            return View(movies);
        }

        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult Begin()
        {
            return View();
        }

        
    }
}