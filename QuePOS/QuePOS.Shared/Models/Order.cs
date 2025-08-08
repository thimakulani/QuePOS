using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuePOS.Shared.Models
{
    public class Order
    {

        [Key]
        public Guid Id { get; set; }
        public Guid? StoreUserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CashReceived { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ChangeAmount { get; set; }
        public Guid? StoreId { get; set; }
        public Guid? CustomerId { get; set; }
        public PaymentType PaymentType { get; set; } = PaymentType.Cash;
        public OrderType OrderType { get; set; } = OrderType.Delivery;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public virtual Store Store { get; set; } = null!;
        public virtual StoreUser User { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;

        public virtual ICollection<OrderItems> OrderItems { get; set; } = [];
    }

    public enum PaymentType
    {
        Cash,
        Card,
        OnlinePayment
    }
    public enum OrderStatus
    {
        Deleted,
        Cancelled,
        Accepted,
        Pending,
        Rejected,
        InProgress,
        Completed,
    }
    public enum OrderType
    {
        EatIn,
        Collection,
        Delivery,
        WalkIn,
    };
}
