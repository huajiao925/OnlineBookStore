using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BS.Models
{
    public class OrderDetails
    {
        public int id { get; set; }

        [Required]
        public int OrderId { get; set; }
        [Required]
        public int BookId {get; set;}
        [Required]
        public int Quantity { get; set; }

        //retrieve Order and Product data
        public virtual Order Order { get; set; }
        public virtual Books Books { get; set; }

    }
}
