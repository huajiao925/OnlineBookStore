using BS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BS.DB
{
    public class ShoppingContext : DbContext
    {
        protected IConfiguration configuration;

        public ShoppingContext(DbContextOptions<ShoppingContext> options)
                : base(options)
        {}

        //Create tables
        public DbSet<User> Users { set; get; }
        public DbSet<Books> Books { set; get; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Review> Reviews { set; get; }
        public DbSet<Discount> Discount { set; get; }
        
        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Review>()
                .HasAlternateKey(model => new { model.Id, model.BookId, model.UserId });
        }

    }
}
