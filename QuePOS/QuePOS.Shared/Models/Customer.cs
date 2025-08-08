using System.ComponentModel.DataAnnotations;

namespace QuePOS.Shared.Models
{
	public class Customer
	{
		public Guid Id { get; set; }
		[StringLength(100)]
		public string FirstName { get; set; } = string.Empty;
		[StringLength(100)]
		public string LastName { get; set; } = string.Empty;
		[StringLength(20)]
		public string PhoneNumber { get; set; }
		public string ProfileImage { get; set; }
	}
}
