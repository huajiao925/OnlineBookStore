using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BS.Models;
using BS.DB;
using Microsoft.AspNetCore.Http;
using BS.Models.ViewModels;
using Newtonsoft;
using Microsoft.AspNetCore.Identity;
using Scrypt;
using Microsoft.AspNetCore.Routing;
using Castle.Core.Internal;

namespace BS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShoppingContext shoppingContext;

        public HomeController(ShoppingContext shoppingContext)
        {
            this.shoppingContext = shoppingContext;
        }
        [HttpGet]
        public IActionResult Index()
        {
            /*
            //Get Latest session and pass it to ViewData
            ViewData["itemCount"] = HttpContext.Session.GetString("itemCount");
            ViewData["cartItemList"] = HttpContext.Session.GetString("cartItemSession");
            */

            //all product data
            if (ViewData["Books"] == null)
            {
                List<Books> books = shoppingContext.Books.ToList();
                ViewData["Books"] = books;
            };
 
            List<Review> reviews = shoppingContext.Reviews.ToList();
            ViewData["Reviews"] = reviews;
            //username after logged in
            ViewData["User"] = HttpContext.Session.GetString("User");

            return View();
        }

        public IActionResult Index(string search)
        {
            //after click search, query in the book table and return a list
            List<Books> books = new List<Books>();
            books = shoppingContext.Books.Where(x => x.Name.Contains(search) 
                                                  || x.ISBN.Contains(search)
                                                  || x.Author.Contains(search))
                                                  .ToList();
            
            if(books.IsNullOrEmpty())
            {
                ViewData["message"] = "Opps, no book found.";
            }

            ViewData["Books"] = books;
            List<Review> reviews = shoppingContext.Reviews.ToList();
            ViewData["Reviews"] = reviews;
            return View();
        }



        //Add to Cart Function
        public IActionResult AddToCart(string cmd, string productId)
        {

            if (cmd == "click")
            {
                //in order to get the Books click's count and show the number of count beside the shopping cart icon
                //we use session to get the count and Books (session is string)
                var count = 0;
                if (HttpContext.Session.GetString("itemCount") == null)
                {
                    count = 0;
                }
                else
                {
                    count = int.Parse(HttpContext.Session.GetString("itemCount"));

                }
                count++;
                HttpContext.Session.SetString("itemCount", count.ToString());
                ViewData["itemCount"] = HttpContext.Session.GetString("itemCount");

                //for Books
                //Get value of cart item from session to check the status whether it's null or already existed in cart
                string cartItemInSession = HttpContext.Session.GetString("cartItemSession");
                if (String.IsNullOrEmpty(cartItemInSession))
                {
                    //assigning the items in CartItem new object
                    List<CartItem> cartItemList = new List<CartItem>();
                    cartItemList.Add(new CartItem { books = shoppingContext.Books.Find(int.Parse(productId)), Quantity = 1 });

                    //update in session again, and Session only accept string which is why need to convert to Json string
                    string cartItemListJson = Newtonsoft.Json.JsonConvert.SerializeObject(cartItemList);
                    HttpContext.Session.SetString("cartItemSession", cartItemListJson);
                }
                else
                {
                    //as it is not null, getting the json item list from session
                    string itemInSession = HttpContext.Session.GetString("cartItemSession");

                    //Deserialize json which is converting string to ----> object list (return type)
                    List<CartItem> carts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartItem>>(itemInSession);

                    //if item already exist --> only Increase Quantity
                    if (carts.Count > 0)
                    {
                        //setting index -1, using this to decide which line code to execute -----> it will only allow those products which are not same as the items in cart to be added in the list
                        //if index change ----> not equal to index -1 => then only we will add the quantity
                        int index = -1;
                        for (int i = 0; i < carts.Count; i++)
                        {
                            if (carts[i].books.id == int.Parse(productId))
                            {
                                index = i;
                            }
                        }
                        if (index != -1)
                        {
                            carts[index].Quantity++;
                        }
                        else
                        {
                            carts.Add(new CartItem { books = shoppingContext.Books.Find(int.Parse(productId)), Quantity = 1 });
                        }
                    }

                    //store the value in session in json format
                    string cartItemJson = Newtonsoft.Json.JsonConvert.SerializeObject(carts);
                    HttpContext.Session.SetString("cartItemSession", cartItemJson);
                }

                ViewData["cartItemList"] = HttpContext.Session.GetString("cartItemSession");
            }
            return RedirectToAction("Index");
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        //View Cart page
        public IActionResult ViewCart()
        {
            ViewData["itemCount"] = HttpContext.Session.GetString("itemCount");
            ViewData["cartItemList"] = HttpContext.Session.GetString("cartItemSession");
            ViewData["logonUser"] = HttpContext.Session.GetString("logonUser");
            return View();
        }

        //Update Cart Function
        public IActionResult UpdateCart()
        {
            // formData ---> getting string value from View(ViewCart) side
            var FormData = HttpContext.Request.Form;

            //storing data in array type for two data entry
            string[] productIds = null;
            string[] quantities = null;
            //added totalCost
            string totalCost = Request.Form["totalCost"];

            foreach (var items in FormData)
            {
                //check each data using 'Contain" and assigning respestively
                if (items.Key.Contains("productId"))
                {
                    productIds = items.Value;
                }
                else if (items.Key.Contains("quantity"))
                {
                    quantities = items.Value;
                }
            }

            //after storing data in each of the array list, we will be assigning the data in our ViewModel CartItem 
            if (productIds != null)
            {
                //instantiating the new object from CartItem
                List<CartItem> cartItems = new List<CartItem>();

                for ( int i = 0; i < productIds.Length; i++)
                {
                    //this line of code is to check if the quantity is 0, then the next line of code will not be executed.
                    //meaning the Books item will not be displayed in the cart
                    if(int.Parse(quantities[i]) != 0)
                    {
                        //instantiating each of the Books object in order to assign the value in cartItems
                        cartItems.Add(new CartItem { books = shoppingContext.Books.Find(int.Parse(productIds[i])), Quantity = int.Parse(quantities[i]) });
                    } 
                }

                //changing int to Json string type in order to update the data in Session
                string cartItemList;
                if (cartItems.Count != 0)
                {
                    cartItemList = Newtonsoft.Json.JsonConvert.SerializeObject(cartItems);
                    HttpContext.Session.SetString("cartItemSession", cartItemList);
                    //set session for totalCost
                    HttpContext.Session.SetString("totalCost", totalCost);
                }
                else
                {
                    cartItemList = null;
                    HttpContext.Session.Remove("cartItemSession");
                }
                
                
            }

            //this code is to get the total number of Books quantities in the cart
            //as quantities is assign as string array value -----> use format of Array.CovertAll in order to convert integer
            //then the sum of quantities id is the total count of products in cartItems
            if (quantities != null)
            {
                int[] count = Array.ConvertAll(quantities, int.Parse);
                HttpContext.Session.SetString("itemCount", count.Sum().ToString());
            }
           
           

            ViewData["itemCount"] = HttpContext.Session.GetString("itemCount");
            ViewData["cartItemList"] = HttpContext.Session.GetString("cartItemSession");

            return RedirectToAction("ViewCart");
        }

        
        //Checkout Function
        public IActionResult Checkout(string cmd)
        {
            //check login status first, if not login, direct to login page.
            string userInSession = HttpContext.Session.GetString("logonUser");
            if (userInSession == null)
            {
                return RedirectToAction("Login", "Home");
            }

            //click Checkout button and direct to CheckoutPage
            if (cmd == "click")
            {
                UpdateCart();
                return RedirectToAction("Index", "Checkout");
            }
            return View();
        }
    }
}
