using Ecommerce_website.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Ecommerce_website.ViewModels
{
    public class NewStoreOwnerViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "The email field is required")]
        [Display(Name ="Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The first name(s) field is required")]
        [Display(Name = "First name(s)")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The phone number field is required")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The street field is required")]
        [Display(Name = "Street")]
        public string StreetNumber { get; set; }

        public string Surbub { get; set; }

        public string City_town { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }

        public string Zip_code { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public IFormFile? ProfilePicture { get; set; }

        public ICollection<StoreResponse> Stores { get; set; } = new List<StoreResponse>();

        public Gender? Gender { get; set; }

        public Ethnicity? Ethnicity { get; set; }

        public Title? Title { get; set; }

        public HomeLanguage? HomeLanguage { get; set; }
    }
}
