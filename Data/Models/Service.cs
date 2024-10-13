using System.ComponentModel.DataAnnotations;

namespace NetPay.Data.Models
{
    public class Service
    {
        public Service()
        {
            Expenses = new HashSet<Expense>();
            SuppliersServices = new HashSet<SupplierService>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string ServiceName { get; set; }

        public ICollection<Expense> Expenses { get; set; }
        public ICollection<SupplierService> SuppliersServices { get; set; }
    }
}