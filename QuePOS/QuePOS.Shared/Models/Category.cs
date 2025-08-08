using System.ComponentModel.DataAnnotations;

namespace QuePOS.Shared.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;
        [StringLength(100)]
        public string Description { get; set; } = string.Empty;

        // Relationships
    }

}
