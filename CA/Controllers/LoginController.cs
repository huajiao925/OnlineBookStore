using BookStore.Services;
using BS.DB;
using BS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BS.Controllers
{
    public class LoginController: Controller
    {
        private ShoppingContext shoppingContext;
        protected UserService userService;

        public LoginController(UserService userService, ShoppingContext shoppingContext)
        {
            this.shoppingContext = shoppingContext;
            this.userService = userService;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult MemberLogin()
        {
            var formdata = HttpContext.Request.Form;
            string email = formdata["email"];
            string password = formdata["pass"];

            //use DI to validate and login
            if (userService.UserExists(email, password))
            {
                User user = shoppingContext.Users.Where(x => x.Email == email).FirstOrDefault();
                HttpContext.Session.SetString("User", user.Username);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["errorMsg"] = "Email or password incorrect. Please try again";
                return RedirectToAction("Login");
            }
        }

        //Logout Function
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}

