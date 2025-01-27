using System.ComponentModel.DataAnnotations;

namespace QuePOS.API.Models
{
        public class Store
        {
        [Key]
        public int Id { get; set; } 
            public string StoreName { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public string ImageUrl { get; set; } = string.Empty;
            public string StoreUserId { get; set; } = string.Empty;  
            
            // Relationships
            public ICollection<StoreUser> StoreUsers { get; set; } = [];
            public ICollection<Product> Products { get; set; } = [];
            public ICollection<Sale> Sales { get; set; } = [];
        }

}
