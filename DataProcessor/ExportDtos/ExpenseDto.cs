using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NetPay.DataProcessor.ExportDtos
{
    [XmlType("Expense")]
    public class ExpenseDto
    {
        [XmlElement("ExpenseName")]
        public string ExpenseName { get; set; }

        [XmlElement("Amount")]
        public string Amount { get; set; }

        [XmlElement("PaymentDate")]
        public string PaymentDate { get; set; }

        [XmlElement("ServiceName")]
        public string ServiceName { get; set; }
        
    }
}
