using Boardgames.Helpers;
using Microsoft.EntityFrameworkCore;
using NetPay.Data;
using NetPay.Data.Models;
using NetPay.Data.Models.Enums;
using NetPay.DataProcessor.ExportDtos;
using Newtonsoft.Json;
using System.Text.Json;
using System.Xml.Serialization;

namespace NetPay.DataProcessor
{
    public class Serializer
    {
        public static string ExportHouseholdsWhichHaveExpensesToPay(NetPayContext context)
        {


            HouseholdDto[] householdExportData = context.Households
            .Where(h => h.Expenses.Any(e => e.PaymentStatus != PaymentStatus.Paid))
            .Select(h => new HouseholdDto()
            {
                ContactPerson = h.ContactPerson,
                Email = h.Email,
                PhoneNumber = h.PhoneNumber,
                Expenses = h.Expenses
                    .Where(e => e.PaymentStatus != PaymentStatus.Paid) // Only unpaid expenses
                    .OrderBy(e => e.DueDate) // Order by due date
                    .ThenBy(e => e.Amount) // Then by amount if dates are the same
                    .Select(e => new ExpenseDto()
                    {
                        ExpenseName = e.ExpenseName,
                        Amount = e.Amount.ToString("F2"), // Format amount to two decimal places
                        PaymentDate = e.DueDate.ToString("yyyy-MM-dd"), // Format due date
                        ServiceName = e.Service.ServiceName // Assuming Service has ServiceName
                    }).ToList()
            })
            .OrderBy(h => h.ContactPerson) // Order households by contact person
            .ToArray();
            return XmlSerializationHelper.Serialize(householdExportData, "Households");
        }



        public static string ExportAllServicesWithSuppliers(NetPayContext context)
        {
            var services = context.Services
                   .Include(s => s.SuppliersServices)
                   .ThenInclude(ss => ss.Supplier)
                   .AsEnumerable() // Fetch the data first and switch to in-memory processing
                   .Select(s => new
                   {
                       ServiceName = s.ServiceName,
                       Suppliers = s.SuppliersServices
                           .Select(ss => ss.Supplier)
                           .OrderBy(sup => sup.SupplierName, StringComparer.CurrentCultureIgnoreCase) // Case-insensitive alphabetical sort in-memory
                           .Select(sup => new
                           {
                               SupplierName = sup.SupplierName
                           })
                           .ToList()
                   })
                   .OrderBy(s => s.ServiceName, StringComparer.CurrentCultureIgnoreCase) // Case-insensitive alphabetical sort in-memory
                   .ToList();

            return JsonConvert.SerializeObject(services, Formatting.Indented);
        }
    }
}
