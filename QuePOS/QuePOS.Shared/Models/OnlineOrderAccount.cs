using System.Runtime.CompilerServices;

namespace QuePOS.Shared.Models
{
    public class OnlineOrderAccount
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public string PayStackCode { get; set; }
        public bool IsVerified { get; set; }
    }
}
