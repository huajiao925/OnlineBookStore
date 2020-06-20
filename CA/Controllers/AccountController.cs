using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BS.DB;
using BS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BS.Controllers
{
    public class AccountController : Controller
    {
        protected ShoppingContext shoppingContext;
        // GET: Purchases
        public AccountController(ShoppingContext shoppingContext)
        {
            this.shoppingContext = shoppingContext;
        }
                
        public IActionResult Index()
        {
            
            List<Books> products = shoppingContext.Books.ToList();
            List<OrderDetails> orders = shoppingContext.OrderDetails.ToList();
            List<Discount> discounts = shoppingContext.Discount.ToList();

            ViewData["itemCount"] = HttpContext.Session.GetString("itemCount");
            ViewData["cartItemList"] = HttpContext.Session.GetString("cartItemSession");

            //Load Purchases according to Session UserId
            ViewData["logonUser"] = HttpContext.Session.GetString("logonUser");
            var logonUser = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(@ViewData["logonUser"].ToString());
            int user = logonUser.id;
            
            
            List<OrderDetails> userOrders = orders.Where(x => x.Order.UserId.Equals(user)).ToList();

            
            List<Discount> discount = discounts.Where(x => x.Order.UserId.Equals(user)).ToList();
            //ViewData["PurchaseHistory"] = userOrders; 

            ViewData["OrderDetails"] = userOrders;
            ViewData["Discount"] = discounts;
            
            
            //List<int> noCode = userOrders.Select(x => x.Quantity).ToList();
            //List<int>  = userOrders.Select(x => x.Quantity).ToList();

            return View();
        }

       
    }
}