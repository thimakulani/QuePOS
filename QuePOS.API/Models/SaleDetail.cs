using System.ComponentModel.DataAnnotations;

namespace QuePOS.API.Models
{
        public class SaleDetail
        {
        [Key]
        public int Id { get; set; }
            public int SaleID { get; set; }
            public int? ProductID { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal Total { get; set; }

            // Relationships
            public Sale Sale { get; set; } = null!;
            public Product Product { get; set; } = null!;
        }
}
