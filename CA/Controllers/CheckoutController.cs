using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BS.Services;
using BS.DB;
using BS.Models;
using BS.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace BS.Controllers
{
    public class CheckoutController : Controller
    {
        protected ShoppingContext shoppingContext;

        public CheckoutController(ShoppingContext shoppingContext)
        {
            this.shoppingContext = shoppingContext;
        }
        public IActionResult Index(string cmd)
        {
            ViewData["logonUser"] = HttpContext.Session.GetString("logonUser");
            //retrieve totalCost from ViewCart session
            ViewData["totalCost"] = HttpContext.Session.GetString("totalCost");
            if (cmd == "paynow")
            {
                AddOrder();
                //clear cart session 
                HttpContext.Session.Remove("itemCount");
                alert();
            }
            return View();
        }


        //add new Order to table
        public void AddOrder()
        {

            //get data from session
            string userInSession = HttpContext.Session.GetString("logonUser");
            int userId = int.Parse(userInSession[6].ToString());
            int totalAmount = int.Parse(HttpContext.Session.GetString("totalCost"));

            //add a new Order
            Order newOrder = new Order()
            {
                UserId = userId,
                TotalAmount = totalAmount,
                PurchaseDate = DateTime.UtcNow
            };

            //shoppingContext.Order.Add(newOrder);
            shoppingContext.SaveChanges();
            AddOrderDetails(newOrder);

            //begin transaction (and assume ít succeed)
            using (IDbContextTransaction transact = shoppingContext.Database.BeginTransaction())
            {
                try
                {
                    PaymentGateway payment = new PaymentGateway();
                    if (payment.Pay(newOrder) == false)
                        throw new Exception("Payment failed.");
                    transact.Commit();
                }
                catch (Exception)
                {
                    transact.Rollback();
                }
            }

            return;
        }

        //add the OrderDetials to table
        public void AddOrderDetails(Order newOrder)
        {
            string itemInSession = HttpContext.Session.GetString("cartItemSession");
            List<CartItem> carts = Newtonsoft.Json.JsonConvert
                .DeserializeObject<List<CartItem>>(itemInSession);

            foreach (var item in carts)
            {
                //add the OrderDetails with the OrderId generated
                OrderDetails newOrderDetails = new OrderDetails()
                {
                    Order = newOrder,
                    BookId = item.books.id,
                    Quantity = item.Quantity
                };
                shoppingContext.OrderDetails.Add(newOrderDetails);
                shoppingContext.SaveChanges();

                //for each order & Books, generate ActivationCode for each Books purchased;
                for (int i = 0; i < newOrderDetails.Quantity; i++)
                {
                    Discount newCode = new Discount()
                    {
                        OrderId = newOrder.id,
                        DiscountCodes = Guid.NewGuid().ToString()
                    };
                    shoppingContext.Discount.Add(newCode);
                    shoppingContext.SaveChanges();
                }
            }
            //after payment done, clear shopping cart session.
            HttpContext.Session.Remove("cartItemSession");
            return;
        }

        
        public IActionResult toMyPurchase(string cmd)
        {
            //if clicked ViewMyPurchase, redirect to MyPurchase Page (update controlle/action path)
            if (cmd == "toMyPurchase")
                return RedirectToAction("Index", "Purchases");
            return View();
            
        }
        //Alert when payment is done.
        public IActionResult alert()
        {
            ViewBag.Message = "Wow, Your Payment Succeed!";
            return View();
        }
    }
}

