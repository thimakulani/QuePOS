using System.ComponentModel.DataAnnotations;

namespace QuePOS.Shared.Models
{
    public class Store
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? ParentStoreId { get; set; }
        [StringLength(100)]

        public string StoreName { get; set; } = string.Empty;
        [StringLength(100)]

        public string Address { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        [StringLength(15)]

        public string Phone { get; set; } = string.Empty;
        [StringLength(250)]

        public string ImageUrl { get; set; } = string.Empty;
        [StringLength(100)]

        public string StoreUserId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public bool AcceptOnlineOrders { get; set; } = false;
        public bool AcceptDeliveries { get; set; } = false;

        public virtual ICollection<Store> SubStores { get; set; } = [];
        // Relationships
        //public ICollection<StoreUser> StoreUsers { get; set; } = [];
        public ICollection<Product> Products { get; set; } = [];
        public ICollection<Order> Sales { get; set; } = [];
    }

}
