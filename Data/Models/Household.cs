using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPay.Data.Models
{
    public class Household
    {
        public Household()
        {
            Expenses = new HashSet<Expense>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ContactPerson { get; set; }

    
        [MaxLength(80)]
  
        public string? Email { get; set; }

        [Required]
        [StringLength(15)]
        [RegularExpression(@"^\+\d{3}/\d{3}-\d{6}$")]
        public string PhoneNumber { get; set; }

        public ICollection<Expense> Expenses { get; set; }
    }
}
