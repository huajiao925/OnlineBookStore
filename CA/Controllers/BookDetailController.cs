using BS.DB;
using BS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class BookDetailController : Controller
    {
        private readonly ShoppingContext shoppingContext;
        public BookDetailController(ShoppingContext shoppingContext)
        {
            this.shoppingContext = shoppingContext;
        }
        [Route("/bookdetail/{bookISBN}")]
        public IActionResult Index(string bookISBN)
        {
            
            Books book = shoppingContext.Books.Where(x => x.ISBN == bookISBN).FirstOrDefault();
            List<Review> reviewList = shoppingContext.Reviews.Where(y => y.Book.ISBN == bookISBN).ToList();
            ViewData["bookInfo"] = book;
            ViewData["reviewInfo"] = reviewList;
            return View();
        }
    }

    

}
