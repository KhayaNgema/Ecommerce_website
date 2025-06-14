using System.ComponentModel.DataAnnotations;

namespace Ecommerce_website.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email or Phone")]
        public string EmailOrPhone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
