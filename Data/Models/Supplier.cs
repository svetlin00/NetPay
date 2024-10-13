using System.ComponentModel.DataAnnotations;

namespace NetPay.Data.Models
{
    public class Supplier
    {
        public Supplier()
        {
            SuppliersServices = new HashSet<SupplierService>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string SupplierName { get; set; }

        public ICollection<SupplierService> SuppliersServices { get; set; }
    }
}