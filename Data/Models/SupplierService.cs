using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetPay.Data.Models
{
    public class SupplierService
    {

        [Required]
        public int SupplierId { get; set; }

  
        public Supplier Supplier { get; set; }

        [Required]
        public int ServiceId { get; set; }

        public Service Service { get; set; }
    }
}