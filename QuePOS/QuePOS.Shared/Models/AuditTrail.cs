
using System.ComponentModel.DataAnnotations.Schema;

namespace QuePOS.Shared.Models
{
    public class AuditTrail
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null;
        public int? StoreId { get; set; }
        public string Action { get; set; }
        public string TableName { get; set; }
        public int RecordId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Details { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }


        public string Roles { get; set; }
        public string ChangedColumns { get; set; }
        public string Source { get; set; }


        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Store Store { get; set; }
    }
}
