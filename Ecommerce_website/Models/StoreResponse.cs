namespace Ecommerce_website.Models
{
    public class StoreResponse
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreLogo { get; set; }
        public string StoreCode { get; set; }
        public int StoreType { get; set; }
        public bool IsActive { get; set; }
        public bool HasPaid { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        public UserInfo CreatedBy { get; set; }
        public UserInfo ModifiedBy { get; set; }
    }
}
