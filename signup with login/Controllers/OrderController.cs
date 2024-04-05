using signup_with_login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Services.Discovery;

namespace signup_with_login.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {

        SignuploginEntities1Entities db = new SignuploginEntities1Entities();
        // GET: Order
        public ActionResult Index()
        {
            List<ordert> cart = Session["Cart"] as List<ordert>;
            if (cart == null)
            {
                cart = new List<ordert>();
                Session["Cart"] = cart;
            }
            return View(cart);
        }

        public ActionResult Index1()
        {
            var data = db.orderts.ToList();
            return View(data);
        }

        public ActionResult CartView()
        {
            List<ordert> cart = Session["Cart"] as List<ordert>;
            if (cart == null)
            {
                cart = new List<ordert>();
                Session["Cart"] = cart;
            }
            return View(cart);
        }

        

        public ActionResult AddToCart()
        {
            return View();
        }
        // GET: Products/AddToCart
        [HttpPost]
        public ActionResult AddToCart(int productId,  int price , int quantity=1)
        {
            List<ordert> cart = Session["Cart"] as List<ordert> ?? new List<ordert>();

            ordert existingItem = cart.FirstOrDefault(item => item.Id == productId);

            if (existingItem != null)
            {
                existingItem.quantity += quantity;
            }
            else
            {
                cart.Add(new ordert { Id = productId, price = price, quantity = quantity });
            }
           


            Session["Cart"] = cart;


            return RedirectToAction("CartView");


        }

        public ActionResult Payment()
        {
            List<ordert> cart = Session["Cart"] as List<ordert>;
            int itemNo = cart.Count;

            for (int i = 0; i < itemNo; i++)
            {
                var newUser = new ordert
                {
                    Id = i,
                    movie_id = cart[i].Id,
                    cust_id = int.Parse(Session["UserId"].ToString()),
                    price = cart[i].price,
                    total_price = cart[i].price * cart[i].quantity,
                    quantity = cart[i].quantity,
                    order_date = DateTime.Now

                };
                db.orderts.Add(newUser);
                db.SaveChanges();
            }
            Session["Cart"] = null;
            return View();
        }



        //public ActionResult Payment()
        //{
        //    Session["Cart"] = null;
        //    return View();
        //}




        public ActionResult RemoveFromCart()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int productId)
        {
            List<ordert> cart = Session["Cart"] as List<ordert>;

            if (cart != null)
            {

                

                foreach(var item in cart)
                {
                    if(item.Id == productId)
                    {
                        cart.Remove(item);
                        break;
                    }
                }
                Session["Cart"] = cart;
            }
            return RedirectToAction("CartView");
        }

        public ActionResult ClearCart()
        {
            Session.Clear();
            Session.Remove("Cart");

           // Session["Cart"] = "";

            return RedirectToAction("UserDisplay","Movie");
        }
    }
}