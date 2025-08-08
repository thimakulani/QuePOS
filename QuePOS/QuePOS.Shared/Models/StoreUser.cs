using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuePOS.Shared.Models
{
    public class StoreUser
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]

        public string FirstName { get; set; } = string.Empty;
        [StringLength(100)]

        public string LastName { get; set; } = string.Empty;
        [StringLength(100)]

        public string PhoneNumber { get; set; } = string.Empty;
        [StringLength(100)]

        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        [NotMapped]
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        //public bool HasAccess { get; set; } = true;
        // Foreign Key for Store
        public Guid StoreID { get; set; }
        [ForeignKey(nameof(Models.ApplicationUser))]
        public string UserId { get; set; } = null;
        public Store Store { get; set; } = null;
        // Relationships
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Order> Sales { get; set; } = [];
    }

}
