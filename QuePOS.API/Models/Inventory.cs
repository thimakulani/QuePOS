namespace QuePOS.API.Models
{
        public class Inventory
        {
            public int Id { get; set; } 
            public int ProductID { get; set; }
            public string ChangeType { get; set; } = string.Empty; // e.g., "Restock", "Sale"
            public int QuantityChange { get; set; }
            public DateTime ChangeDate { get; set; } = DateTime.Now;

            // Foreign Key for Store
            public int StoreID { get; set; }
            public Store Store { get; set; } = null!;

            // Relationships
            public Product Product { get; set; } = null!;
        }

}
