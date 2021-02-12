using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MvcWeb.Areas.Admin.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Required]
        [Remote("VerifyUserName", "Manage", AreaDefaults.AdminAreaName)]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Display(Name = "Profil Fotoğrafı")]
        public string ProfilePhoto { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        [Remote("VerifyEmail", "Manage", AreaDefaults.AdminAreaName)]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Telefon Numarası")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$", ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz.")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile ProfilePhotoFile { get; set; }
    }
}
