using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuePOS.API.Models
{
    public class SaleDetail
    {
        [Key]
        public int Id { get; set; }
        public int? SaleID { get; set; }
        public int? ProductID { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        // Relationships
        public Sale Sale { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
