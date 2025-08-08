using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuePOS.Shared.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(100)]
        public string BarCode { get; set; } = string.Empty;
        public bool IsAvailableOnline { get; set; } = false;
        public string Description { get; set; } = string.Empty;
        [NotMapped]
        public string Base64Url { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        [StringLength(100)]
        public string PublicId { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }
        public decimal Discount { get; set; }

        public UnitOfMeasure Unit { get; set; } = UnitOfMeasure.Each;
        public double? StockQuantity { get; set; }

        public bool IsDeleted { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid StoreID { get; set; }
        public Store Store { get; set; } = null!;
        public Category Category { get; set; }
    }

    public enum UnitOfMeasure
    {
        Each,     // per unit/item
        Gram,
        Kilogram,
        Liter,
        Small,
        Medium,
        Large
    }

}
