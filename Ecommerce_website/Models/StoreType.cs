using System.ComponentModel.DataAnnotations;

namespace Ecommerce_website.Models
{
    public enum StoreType
    {
        [Display(Name = "Apparel & Accessories")]
        Apparel_And_Accessories,

        [Display(Name = "Home & Living")]
        Home_And_Living,

        [Display(Name = "Health & Beauty")]
        Health_And_Beauty,

        [Display(Name = "Food & Beverage")]
        Food_And_Beverage,

        [Display(Name = "Entertainment & Leisure")]
        Entertainment_And_Leisure,

        [Display(Name = "Sports & Outdoors")]
        Sports_And_Outdoors,

        [Display(Name = "Electronics & Appliances")]
        Electronics_And_Appliances,

        [Display(Name = "Automotive")]
        Automotive,

        [Display(Name = "Hardware & DIY")]
        Hardware_And_DIY,

        [Display(Name = "Baby & Kids")]
        Baby_And_Kids,

        [Display(Name = "Pet Supplies")]
        Pet_Supplies,

        [Display(Name = "Office & Stationery")]
        Office_And_Stationery
    }
}
