namespace QuePOS.Shared.Models;

public class Store
{
    public int Id { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    // Relationships
    public ICollection<StoreUser> Users { get; set; } = new List<StoreUser>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}

