using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BS.Models.ViewModels
{
    public class CartItem
    {
        public Books books { set; get; }

        public int Quantity { set; get; }

        //LL: added to add totalCost into session
        public double totalCost { get; set; }
    }
}
