namespace QuePOS.Shared.Models
{
    public class ApplicationUser
    {
        public int StoreUserId { get; set; }
        public StoreUser StoreUser { get; set; }

    }
}
