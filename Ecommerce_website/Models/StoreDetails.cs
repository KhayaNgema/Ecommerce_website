using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_website.Models
{
    public class StoreDetails
    {
        public int StoreId { get; set; }

        public string StoreName { get; set; }

        public string StoreLogo { get; set; }

        public string StoreCode { get; set; }

        public StoreType StoreType { get; set; }

        public string? SignedContract { get; set; }

        public bool IsSuspended { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }
        public bool HasPaid { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        public UserInfo CreatedBy { get; set; }
        public UserInfo ModifiedBy { get; set; }
    }
}
