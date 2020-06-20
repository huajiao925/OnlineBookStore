using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BS.Models
{
    public class User
    {
        public int id { set; get; }
        
        [Required]
        [MaxLength(200)]
        public string Username { set; get; }
        
        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { set; get; }

        [Required]
        [MaxLength(350)]
        [MinLength(6)]
        public string Password { set; get; }
        
        [Required]
        [MaxLength(10)]
        public bool Status { set; get; }


        public virtual Order Order { get; set;}
       
    }
}
