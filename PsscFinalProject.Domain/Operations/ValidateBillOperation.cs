using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using static PsscFinalProject.Domain.Models.OrderBilling;

namespace PsscFinalProject.Domain.Operations
{
    internal sealed class ValidateBillOperation : OrderBillingOperation
    {
        internal ValidateBillOperation() { }

        protected override IOrderBilling OnUnvalidated(UnvalidatedOrderBilling unvalidatedBills)
        {
            var (validatedBills, validationErrors) = ValidateBills(unvalidatedBills);

            if (validationErrors.Any())
            {
                return new InvalidOrderBilling(unvalidatedBills.BillList, validationErrors);
            }

            return new ValidatedOrderBilling(validatedBills);
        }

        private (List<ValidatedBill>, IEnumerable<string>) ValidateBills(UnvalidatedOrderBilling unvalidatedBills)
        {
            var validationErrors = new List<string>();
            var validatedBills = new List<ValidatedBill>();

            foreach (var bill in unvalidatedBills.BillList)
            {
                var validationResult = ValidateBill(bill);

                if (validationResult.IsValid && validationResult.ValidatedBill != null)
                {
                    validatedBills.Add(validationResult.ValidatedBill);
                }
                else
                {
                    validationErrors.AddRange(validationResult.Errors);
                }
            }

            return (validatedBills, validationErrors);
        }

        private (bool IsValid, ValidatedBill? ValidatedBill, List<string> Errors) ValidateBill(UnvalidatedBill bill)
        {
            var errors = new List<string>();

            var billNumber = ValidateBillNumber(bill.BillNumber, errors);
            var shippingAddress = ValidateShippingAddress(bill.ShippingAddress, errors);
            var totalAmount = ValidateTotalAmount(bill.TotalAmount, errors);

            if (!errors.Any() && billNumber != null && shippingAddress != null && totalAmount != null)
            {
                return (true, new ValidatedBill(billNumber, shippingAddress, totalAmount), errors);
            }

            return (false, null, errors);
        }

        private BillNumber? ValidateBillNumber(string billNumber, List<string> errors)
        {
            if (!BillNumber.TryParse(billNumber, out var validBillNumber))
            {
                errors.Add($"Invalid bill number: {billNumber}");
                return null;
            }

            return validBillNumber;
        }

        private ShippingAddress? ValidateShippingAddress(string shippingAddress, List<string> errors)
        {
            if (!ShippingAddress.TryParse(shippingAddress, out var validAddress))
            {
                errors.Add($"Invalid shipping address: {shippingAddress}");
                return null;
            }

            return validAddress;
        }

        private ProductPrice? ValidateTotalAmount(decimal totalAmount, List<string> errors)
        {
            if (!ProductPrice.TryParse(totalAmount, out var validPrice))
            {
                errors.Add($"Invalid total amount: {totalAmount}");
                return null;
            }

            return validPrice;
        }
    }
}
