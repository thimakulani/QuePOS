namespace QuePOS.API.Models
{
    public partial class Entities
    {
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public int CategoryID { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.Now;

            // Foreign Key for Store
            public int StoreID { get; set; }
            public Store Store { get; set; } = null!;

            // Relationships
            public Category Category { get; set; } = null!;
            public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
            public ICollection<Inventory> InventoryChanges { get; set; } = new List<Inventory>();
        }

    }
}
