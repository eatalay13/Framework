using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcWeb.Areas.Identity.Models
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Kullanıcı Adınız")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Adınız")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Soyadınız")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
