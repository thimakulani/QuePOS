namespace QuePOS.Shared.Models;

public class Store
{
    public int Id { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    // Relationships
    public ICollection<StoreUser> Users { get; set; } = [];
    public ICollection<Product> Products { get; set; } = [];
    public ICollection<Sale> Sales { get; set; } = [];
    public ICollection<Inventory> Inventories { get; set; } = [];
}

