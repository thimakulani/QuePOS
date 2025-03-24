using System.ComponentModel.DataAnnotations.Schema;

namespace QuePOS.Shared.Models
{
    public class StoreUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        //public bool HasAccess { get; set; } = true;
        // Foreign Key for Store
        public int StoreID { get; set; }

        public string UserId { get; set; } = null;

    }

}
