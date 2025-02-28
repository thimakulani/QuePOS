using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuePOS.API.Models
{
    public class AuditTrail
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Models.ApplicationUser))]
        public string UserId { get; set; } = null;
        [ForeignKey(nameof(Models.Store))]
        public int? StoreId { get; set; }
        public string Action { get; set; }
        public string TableName { get; set; }
        public int RecordId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Details { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Store Store { get; set; }
    }
}
