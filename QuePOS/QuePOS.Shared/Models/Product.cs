namespace QuePOS.Shared.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BarCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Base64Url { get; set; } = string.Empty;
    public double? Price { get; set; }
    public int? StockQuantity { get; set; }
    public bool IsDeleted { get; set; }
    public int CategoryID { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Foreign Key for Store
    public int StoreID { get; set; }
    public Store Store { get; set; } = null!;

    // Relationships
    public Category Category { get; set; } = null!;
    public ICollection<SaleDetail> SaleDetails { get; set; } = [];
    public ICollection<Inventory> InventoryChanges { get; set; } = [];
}

