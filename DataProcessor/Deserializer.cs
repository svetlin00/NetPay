using Boardgames.Helpers;
using Microsoft.EntityFrameworkCore;
using NetPay.Data;
using NetPay.Data.Models;
using NetPay.DataProcessor.ImportDtos;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft;
using NetPay.Data.Models.Enums;
using System.Globalization;
using Microsoft.VisualBasic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NetPay.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedHousehold = "Successfully imported household. Contact person: {0}";
        private const string SuccessfullyImportedExpense = "Successfully imported expense. {0}, Amount: {1}";

     
        public static string ImportHouseholds(NetPayContext context, string xmlString)
        {
            ImportHouseholdDto[] householdDtos = XmlSerializationHelper.Deserialize<ImportHouseholdDto[]>(xmlString, "Households");
            List<Household> householdList = new List<Household>();
            StringBuilder stringBuilder = new StringBuilder();


            foreach (var housegoldDto in householdDtos.Distinct())
            {
                 
                 bool itsDuplicate = householdList.Any(h =>
             h.ContactPerson == housegoldDto.ContactPerson ||
             h.Email == housegoldDto.Email ||
             h.PhoneNumber == housegoldDto.Phone);

          
              
                

                    if (!IsValid(housegoldDto))
                    {
                        stringBuilder.AppendLine(ErrorMessage);
                        continue;
                    }


                    if (itsDuplicate)
                    {
                        stringBuilder.AppendLine(DuplicationDataMessage);
                        continue;
                    }

                    Household household = new Household
                    {
                        ContactPerson = housegoldDto.ContactPerson,
                        Email = housegoldDto.Email,
                        PhoneNumber = housegoldDto.Phone

                    };

                    stringBuilder.AppendLine(string.Format(SuccessfullyImportedHousehold, household.ContactPerson));

                    householdList.Add(household);
                

            }
            context.AddRange(householdList);
            context.SaveChanges();



            return stringBuilder.ToString();
        }

        public static string ImportExpenses(NetPayContext context, string jsonString)
        {
            ImportExpenseDto[] importExpenseDtos = JsonConvert.DeserializeObject<ImportExpenseDto[]>(jsonString);
            List<Expense> expenseList = new List<Expense>();
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var importExpenseDto in importExpenseDtos.Distinct())
            {
                if (importExpenseDto.ExpenseName == null && importExpenseDto.PaymentStatus == null && importExpenseDto.DueDate == null && importExpenseDto.HouseholdId == null && importExpenseDto.ServiceId == null && importExpenseDto.Amount == null) {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }


                if (!IsValid(importExpenseDto))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }


             

                var householdExists = expenseList.Any(h => h.Id == importExpenseDto.HouseholdId);
                var serviceExists = expenseList.Any(s => s.Id == importExpenseDto.ServiceId);
                bool existingHouseholdId = context.Households.Any(h => h.Id == importExpenseDto.HouseholdId);
                bool existingServiceId = context.Services.Any(c => c.Id == importExpenseDto.ServiceId);

                if (!existingHouseholdId)
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue ;
                }
                if (!existingServiceId) 
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }
                var expense = new Expense
                {
                    ExpenseName = importExpenseDto.ExpenseName,
                    Amount = importExpenseDto.Amount,
                    HouseholdId = importExpenseDto.HouseholdId,
                    ServiceId = importExpenseDto.ServiceId
                };

                if (householdExists || serviceExists)
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }
                DateTime date;
                bool validDate = DateTime.TryParseExact(importExpenseDto.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                if (validDate)
                {

                    expense.DueDate = date;
                }
                else
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }





                if (Enum.TryParse(importExpenseDto.PaymentStatus, out PaymentStatus paymentStatus))
                {
                    expense.PaymentStatus = paymentStatus;
                }
                else
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }


                expenseList.Add(expense);
                stringBuilder.AppendLine(string.Format(SuccessfullyImportedExpense, importExpenseDto.ExpenseName, importExpenseDto.Amount.ToString("F2")));
            
            }
        
            context.AddRange(expenseList);
            context.SaveChanges();
            return stringBuilder.ToString();

        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            foreach(var result in validationResults)
            {
                string currvValidationMessage = result.ErrorMessage;
            }

            return isValid;
        }
    }
}
