using BS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BS.Models
{
    public class Books
    {
        public int id { set; get; }

        [Required]
        [MaxLength(100)]
        public string Name { set; get; }

        [Required]
        [MaxLength(100)]
        public string ISBN { set; get; }

        [Required]
        [MaxLength(100)]
        public string Author { set; get; }

        [Required]
        [MaxLength(10)]
        public long PublishYear { set; get; }

        [MaxLength(500)]
        public string Description { set; get; }

        [Required]
        public int UnitPrice { set; get; }

        public virtual Review Review { get; set; }

    }
}
