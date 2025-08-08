using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuePOS.Shared.Models
{
    public class OrderItems
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public Guid? OrderId { get; set; }
        [ForeignKey(nameof(Models.Product))]
        public Guid? ProductId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public virtual Product Product { get; set; } = new();
    }
}
