using Ecommerce_website.Models;
using System.Reflection;

namespace Ecommerce_website.ViewModels
{
    public class NewStoreOwnerViewModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string PhoneNumber { get; set; }

        public string StreetNumber { get; set; }

        public string Surbub { get; set; }

        public string City_town { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }

        public string Zip_code { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public IFormFile? ProfilePicture { get; set; }

        public ICollection<StoreResponse> Stores { get; set; }

        public Gender? Gender { get; set; }

        public Ethnicity? Ethnicity { get; set; }

        public Title? Title { get; set; }

        public HomeLanguage? HomeLanguage { get; set; }

        public string ReturnUrl { get; set; }
    }
}
