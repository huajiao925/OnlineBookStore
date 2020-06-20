using BookStore.Services;
using BS.DB;
using BS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BS.Controllers
{
    public class SignupController: Controller
    {
        private ShoppingContext shoppingContext;
        protected UserService userService;

        public SignupController(UserService userService, ShoppingContext shoppingContext)
        {
            this.shoppingContext = shoppingContext;
            this.userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        //Create New user
        public IActionResult CreateUser()
        {
            var formdata = HttpContext.Request.Form;
            string pwd = formdata["Password"].ToString();
            string username = formdata["Username"].ToString();
            string Email = formdata["Email"].ToString();

            //user DI to validate and add User
            if (!userService.UserExists(Email, pwd))
            {
                User newUser = new User()
                {
                    Username = username,
                    Password = pwd,
                    Email = Email,
                    Status = true
                };
                userService.AddUser(newUser);
            }
            else
            {
                TempData["errorMessage"] = "User already exists.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index","Login");
        }
    }
}
