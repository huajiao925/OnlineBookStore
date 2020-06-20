using BookStore.Services;
using BS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BS.DB
{
    public class DbSeeder
    {
        private ShoppingContext dbContext;

        public DbSeeder(ShoppingContext dbContext)
        {
            this.dbContext = dbContext;
            //dummy data to test the DB connection, sequence matters.
            Books book = new Books();
            book.Name = "Test Book";
            book.ISBN = "soijdifosidjfsd";
            book.PublishYear = 2000;
            book.Author = "Not me";
            book.UnitPrice = 20;

            Books book1 = new Books();
            book1.Name = "TT Book";
            book1.ISBN = "abcde";
            book1.PublishYear = 2800;
            book1.Author = "Not";
            book1.UnitPrice = 1000;

            Books book2 = new Books();
            book2.Name = "LL Book";
            book2.ISBN = "ssd";
            book2.PublishYear = 2000;
            book2.Author = "Me";
            book2.UnitPrice = 20;

            Books book3 = new Books();
            book3.Name = "AA Book";
            book3.ISBN = "soijdifosidjfsd";
            book3.PublishYear = 2000;
            book3.Author = "Not me";
            book3.UnitPrice = 20;

            Books book4 = new Books();
            book4.Name = "SS Book";
            book4.ISBN = "soijdifosidjfsd";
            book4.PublishYear = 2000;
            book4.Author = "Not me";
            book4.UnitPrice = 2;

            dbContext.Add(book);
            dbContext.Add(book1);
            dbContext.Add(book2);
            dbContext.Add(book3);
            dbContext.Add(book4);

            User user = new User();
            user.Username = "test";
            user.Password = EncryptPwd.Encrypt("123");
            user.Email = "abc@gmail.com";
            user.Status = true;

            User user2 = new User();
            user2.Username = "abc";
            user2.Password = EncryptPwd.Encrypt("123");
            user2.Email = "bcd@gmail.com";
            user2.Status = true;


            dbContext.Add(user);
            dbContext.Add(user2);
            dbContext.SaveChanges();

            Review review1 = new Review()
            {
                BookId = 1,
                UserId = 1,
                Rating = 3.9,
                Reviews = "This book is very informative, especially for a beginner it provides a broad perspective. " +
                "The examples are from real projects and so I could connect with the issues and resolutions quickly and easily."
            };


            Review review2 = new Review()
            {
                BookId = 1,
                UserId = 2,
                Rating = 5.0,
                Reviews = "This book too cheap."
            };
           

            Review review3 = new Review()
            {
                BookId = 3,
                UserId = 2,
                Rating = 3.0,
                Reviews = "This book too expensive."

            };

            dbContext.Add(review1);
            dbContext.SaveChanges();
            dbContext.Add(review2);
            dbContext.SaveChanges();
            dbContext.Add(review3);
            dbContext.SaveChanges();


            Order order1 = new Order();
            order1.UserId = 1;
            order1.TotalAmount = 88;
            order1.PurchaseDate = new DateTime(2020, 2, 12, 18, 00, 00).ToUniversalTime();
            dbContext.Add(order1);

            Order order2 = new Order();
            order2.UserId = 2;
            order2.TotalAmount = 88;
            order2.PurchaseDate = new DateTime(2020, 2, 12, 18, 00, 00).ToUniversalTime();
            dbContext.Add(order2);

            dbContext.SaveChanges();

            OrderDetails orderDetails1 = new OrderDetails()
            {
                OrderId = 1,
                BookId = 2,
                Quantity = 4
            };

            OrderDetails orderDetails2 = new OrderDetails()
            {
                OrderId = 2,
                BookId = 1,
                Quantity = 200
            };

            Discount discount = new Discount()
            {
                DiscountCodes = "testtestabcdefg0",
                OrderId = 1
            };

            dbContext.Add(orderDetails1);
            dbContext.Add(orderDetails2);
            dbContext.Add(discount);
            dbContext.SaveChanges();
        }       
    }

}
