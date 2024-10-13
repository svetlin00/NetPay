using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NetPay.Data.Models.Enums;

namespace NetPay.Data.Models
{
    public class Expense
    {
     

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ExpenseName { get; set; }

        [Required]
        [Range(0.01, 100000)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public PaymentStatus PaymentStatus { get; set; }

        [Required]
        public int HouseholdId { get; set; }

        [ForeignKey(nameof(HouseholdId))]
        public Household? Household { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public Service? Service { get; set; }
    }
}