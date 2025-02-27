
namespace QuePOS.Shared.Models
{
    public class AuditTrail
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null;
        public string Action { get; set; }
        public string TableName { get; set; }
        public int RecordId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Details { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
