using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BS.Models
{
    public class Review
    {
        public int Id { set; get; }
        [Required]
        public int BookId { set; get; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public double Rating { set; get; }

        [MaxLength(600)]
        public string Reviews { set; get; }

        public virtual Books Book { get; set; }
        public virtual User User { get; set; }
    }
}
