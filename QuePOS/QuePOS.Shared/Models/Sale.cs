namespace QuePOS.Shared.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }

        // Foreign Key for Store
        public int StoreID { get; set; }
        public Store Store { get; set; } = null!;

        // Relationships
        public StoreUser User { get; set; } = null!;
        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    }

}
