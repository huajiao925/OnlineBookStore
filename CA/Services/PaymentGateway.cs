using BS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BS.Services
{
    public class PaymentGateway
    {
        public bool Pay(Order order)
        {
            return true;
        }
    }
}
