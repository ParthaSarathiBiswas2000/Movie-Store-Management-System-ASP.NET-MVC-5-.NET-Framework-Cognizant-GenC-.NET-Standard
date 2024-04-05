using signup_with_login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace signup_with_login.Controllers
{
    public class LoginController : Controller
    {

        SignuploginEntities1Entities db = new SignuploginEntities1Entities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(user u, string ReturnUrl)
        {
            var user = db.users.Where(model => model.username == u.username && model.password == u.password).FirstOrDefault();//first default : get and store in user of var type
            if(user != null)
            {
                FormsAuthentication.SetAuthCookie(u.username, false);
                Session["UserId"] = user.Id.ToString();
                Session["Username"]= user.username.ToString();
                var rl = db.roles.Where(x => x.UserId == user.Id).FirstOrDefault();
                Session["Role"] = rl.Role1;
                TempData["LoginSuccessMessage"] = "<script>alert('Logged in Successfully !!')</script>";
                if (ReturnUrl != null)
                {
                    return Redirect(ReturnUrl);
                }
                else { 
                    return Redirect("/Home/Index"); 
                }
            }
            else
            {
                ViewBag.ErrorMessage = "<script>alert('Username or Password is invalid !!')</script>";
                return View();
            }
            
        }

        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(user u)
        {
            if(ModelState.IsValid == true) { 
                db.users.Add(u);
                int a = db.SaveChanges();
                if(a>0)
                {
                    ViewBag.InsertMessage = "<script>alert('Registered Successfully !!')</script>";
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.InsertMessage = "<script>alert('Registered Failed !!')</script>";
                }
            }
            return View();
        }


        public ActionResult Logout()
        {
            Session["UserId"] = "";
            Session["Username"] = "";
            Session["Role"] = "";
            Session["Cart"] = "";
            return RedirectToAction("Index", "Login");
        }

    }
}