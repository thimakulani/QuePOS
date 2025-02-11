using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuePOS.API.Models
{
    public class StoreUser
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [NotMapped]
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign Key for Store
        public int StoreID { get; set; }
        [ForeignKey(nameof(Models.ApplicationUser))]
        public string UserId { get; set; } = null;
        public Store Store { get; set; } = null;
        // Relationships
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Sale> Sales { get; set; } = [];
    }

}
