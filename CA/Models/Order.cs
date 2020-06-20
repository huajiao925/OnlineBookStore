using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BS.Models
{
    public class Order
    {
        public int id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int TotalAmount { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }

        //retrieve User data
        public virtual User User { get; set; }
    }
}
