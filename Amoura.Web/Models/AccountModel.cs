using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amoura.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class AccountModel
    {
        public int MemberId { get; set; }

        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Bio")]
        [AllowHtml]
        public string Bio { get; set; }

        [Display(Name = "Password"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

    }
}