﻿namespace QuePOS.Shared.Models;

public class Category
{
    public int Id { get; set; } 
    public string CategoryName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Relationships
   // public ICollection<Product> Products { get; set; } = new List<Product>();
}
