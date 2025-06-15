using Ecommerce_website.Models;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_website.ViewModels
{
    public class NewStoreViewModel
    {
        [Required(ErrorMessage ="The store name field is required")]
        [Display(Name = "Store name")]
        public string StoreName { get; set; }

        [Required(ErrorMessage = "The store logo field is required")]
        [Display(Name = "Store logo")]
        public IFormFile StoreLogo { get; set; }

        [Required(ErrorMessage = "The signed contract field is required")]
        [Display(Name = "Signed contract")]
        public IFormFile SignedContract { get; set; }

        [Required(ErrorMessage = "The store code field is required")]
        [Display(Name = "Store code")]
        public string StoreCode { get; set; }

        [Required(ErrorMessage = "The store type field is required")]
        [Display(Name = "Store type")]
        public StoreType StoreType { get; set; }

        [Required(ErrorMessage = "The street field is required")]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required(ErrorMessage = "The city field is required")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "The province field is required")]
        [Display(Name = "Province")]
        public Province Province { get; set; }

        [Required(ErrorMessage = "The postal code field is required")]
        [Display(Name = "Postal/Zip code")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "The country field is required")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "The latitude field is required")]
        [Display(Name = "Latitude")]
        public double? Latitude { get; set; }

        [Required(ErrorMessage = "The longitude name field is required")]
        [Display(Name = "Longitude")]
        public double? Longitude { get; set; }
    }
}
