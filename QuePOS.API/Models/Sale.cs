using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuePOS.API.Models
{
    public class Sale
    {

        [Key]
        public int Id { get; set; }
        public int StoreUserId { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Foreign Key for Store
        public int? StoreID { get; set; }
        public Store Store { get; set; } = null!;

        // Relationships
        public StoreUser User { get; set; } = null!;
        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    }

}
