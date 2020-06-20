using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BS.Models
{
    public class Discount
    {
        public int id { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string DiscountCodes { get; set; }
        [Required]
        public int OrderId { get; set; }

        //retrieve Order data
        public virtual Order Order { get; set; }
    }
}
