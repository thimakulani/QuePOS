namespace QuePOS.Shared.Models
{
    public class SaleDetail
    {
        public int Id { get; set; }
        public int SaleID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public decimal Total { get; set; }


        // Relationships
        public Sale Sale { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }

}
