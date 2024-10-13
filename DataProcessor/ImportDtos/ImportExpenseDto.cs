using Microsoft.VisualBasic;
using NetPay.Data.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using JsonConverterAttribute = System.Text.Json.Serialization.JsonConverterAttribute;

namespace NetPay.DataProcessor.ImportDtos
{
    public class ImportExpenseDto
    {
        [Required]
        [JsonProperty("ExpenseName")]
        [StringLength(50, MinimumLength = 5)]
        public string ExpenseName { get; set; }

        [JsonProperty("Amount")]
        [Required]
        [Range(0.01, 100000)]
        public decimal Amount { get; set; }

        [JsonProperty("DueDate")]
        [Required]
        public string DueDate { get; set; }

        [Required]
        [JsonProperty("PaymentStatus")]
        public  string PaymentStatus { get; set; }


        [JsonProperty("HouseholdId")]
        [Required]
        public int HouseholdId { get; set; }

        [JsonProperty("ServiceId")]
        [Required]
        public int ServiceId { get; set; }
    }
}
