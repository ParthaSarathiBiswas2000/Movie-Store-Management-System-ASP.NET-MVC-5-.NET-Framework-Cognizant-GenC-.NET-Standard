using signup_with_login.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace signup_with_login.Controllers
{
    //[Authorize]
    public class MovieController : Controller
    {

        SignuploginEntities1Entities db = new SignuploginEntities1Entities();
        // GET: Movie


        public ActionResult Index(string searchBy, string search)
        {
            if (searchBy == "name")
            {
                var data = db.movies.Where(model => model.name == search).ToList();
                return View(data);
            }
            else
            {
                var data = db.movies.ToList();
                return View(data);
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }


        [Authorize(Roles ="Admin")]
        [HttpPost]
        public ActionResult Create(movie m)
        {
                string moviename = Path.GetFileNameWithoutExtension(m.ImageFile.FileName);
                string extension = Path.GetExtension(m.ImageFile.FileName);
                moviename = moviename + extension;
                m.image_path = "~/images/" + moviename;
                moviename = Path.Combine(Server.MapPath("~/images/"), moviename);
                m.ImageFile.SaveAs(moviename);
                db.movies.Add(m);
                int a = db.SaveChanges();

                if (a > 0)
                {
                    TempData["Success"] = "<script>alert('Record Inserted !!!')</script>";
                    ModelState.Clear();
                    return RedirectToAction("Index", "Movie");
                }
                else
                {
                    TempData["Failed"] = "<script>alert('Record Not Inserted !!!')</script>";
                }
       




            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var MovieRow = db.movies.Where(model => model.Id == id).FirstOrDefault();
            Session["Image"] = MovieRow.image_path;
            return View(MovieRow);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(movie m)
        {
                if(m.ImageFile != null) {
                    string moviename = Path.GetFileNameWithoutExtension(m.ImageFile.FileName);
                    string extension = Path.GetExtension(m.ImageFile.FileName);
                    moviename = moviename + extension;
                    m.image_path = "~/images/" + moviename;
                    moviename = Path.Combine(Server.MapPath("~/images/"), moviename);
                    m.ImageFile.SaveAs(moviename);
                    db.Entry(m).State = EntityState.Modified;
                    int a = db.SaveChanges();

                    if (a > 0)
                    {
                        TempData["UpdateMessage"] = "<script>alert('Record Updated !!!')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "Movie");
                    }
                    else
                    {
                        TempData["FailedMessage"]= "<script>alert('Record Not Updated !!!')</script>";
                    }
                }
                else
                {
                    m.image_path = Session["Image"].ToString();
                    db.Entry(m).State = EntityState.Modified;
                    int a = db.SaveChanges();

                    if (a > 0)
                    {
                        TempData["UpdateMessage"] = "<script>alert('Record Updated !!!')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "Movie");
                    }
                    else
                    {
                        TempData["FailedMessage"] = "<script>alert('Record Not Updated !!!')</script>";
                    }
                }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            if(id>0)
            {
                var MovieRow = db.movies.Where(model => model.Id == id).FirstOrDefault();

                if(MovieRow != null)
                {
                    db.Entry(MovieRow).State = EntityState.Deleted;
                    int a = db.SaveChanges();
                    if(a > 0)
                    {
                        TempData["DeleteMessage"] = "<script>alert('Data Deleted Successfully !!!')</script>";
                        string ImagePath = Request.MapPath(MovieRow.image_path.ToString());
                        if (System.IO.File.Exists(ImagePath))
                        {
                            System.IO.File.Delete(ImagePath);
                        }
                    }
                    else
                    {
                        TempData["DeleteMessage"] = "<script>alert('Data Deletion Failed !!!')</script>";
                    }
                }
            }
            return RedirectToAction("Index","Movie");
        }

        [Authorize(Roles ="Customer")]
        public ActionResult UserDisplay()
        {
            return View(db.movies.ToList());
        }


    }
}