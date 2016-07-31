using System.ComponentModel.DataAnnotations;

namespace Amoura.Web.Models
{
    public class MemberLoginModel
    {
        [Required, Display(Name = "Email")]
        public string Username { get; set; }

        [Required, Display(Name = "Password"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}