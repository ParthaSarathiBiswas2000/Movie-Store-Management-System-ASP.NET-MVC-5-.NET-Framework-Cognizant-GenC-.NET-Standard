using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace signup_with_login.Models
{
    public class Items_InCart
    {
        //public int ProductId { get; set; }

        //public string ProductName { get; set; }
        public movie Product { get; set; }
        public user user { get; set; }
        public int price { get; set; }
        public int Quantity { get; set; }
        public int TotalAmount { get; set; }
    }
}