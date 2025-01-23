namespace QuePOS.API.Models
{
    public partial class Entities
    {
        public class StoreUser 
        {
            public int Id { get; set; } 
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty; 
            public DateTime CreatedAt { get; set; } = DateTime.Now;

            // Foreign Key for Store
            public int StoreID { get; set; }
            public Store Store { get; set; } = null!;

            // Relationships
            public ICollection<Sale> Sales { get; set; } = new List<Sale>();
        }

    }
}
