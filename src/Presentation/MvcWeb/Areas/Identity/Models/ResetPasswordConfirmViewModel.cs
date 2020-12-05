using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcWeb.Areas.Identity.Models
{
    public class ResetPasswordConfirmViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string NewPasswordConfirm { get; set; }
    }
}
