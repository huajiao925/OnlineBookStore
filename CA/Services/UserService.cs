using BS.DB;
using BS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class UserService
    {
        private ShoppingContext shoppingContext;

        public UserService(ShoppingContext shoppingContext)
        {
            this.shoppingContext = shoppingContext;
        }

        public void AddUser(User user)
        {
            if (UserExists(user.Email,user.Password)) {
                shoppingContext.Users.Add(user);
                shoppingContext.SaveChanges();
            }
        }

        public bool UserExists(string email, string password)
        {
            string hash_pass = EncryptPwd.Encrypt(password);
            var validUser = shoppingContext.Users.Where(x => x.Email == email && x.Password == hash_pass).FirstOrDefault();
            if (validUser != null)
                return true;
            return false;
        }
    }
}
