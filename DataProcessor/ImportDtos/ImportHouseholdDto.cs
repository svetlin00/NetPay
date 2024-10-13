using NetPay.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NetPay.DataProcessor.ImportDtos
{
    [XmlType("Household")]
    public class ImportHouseholdDto
    {
        [XmlAttribute("phone")]
        [Required]
        [StringLength(15)]
        [RegularExpression(@"^\+\d{3}/\d{3}-\d{6}$")]
        public string Phone { get; set; }

        [XmlElement("ContactPerson")]
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ContactPerson { get; set; }

        [XmlElement("Email")]
        [MaxLength(60)]
        [MinLength(6)]
        public string? Email { get; set; } 
    }
}
