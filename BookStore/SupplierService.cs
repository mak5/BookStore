using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    public class SupplierService(IDeliveryDateCalculator deliveryDateCalculator, IDateTimeProvider dateTimeProvider) : ISupplierService
    {
        public DateOnly OrderCopies(string title, int copiesCount)
        {
            ValidateInputs(title, copiesCount);

            int businessDaysToAdd = deliveryDateCalculator.CalculateBusinessDaysToAdd(copiesCount);

            var deliveryDate = deliveryDateCalculator.Calculate(businessDaysToAdd);

            if (deliveryDate < DateOnly.FromDateTime(dateTimeProvider.Now))
            {
                throw new InvalidDeliveryDateException();
            }

            return deliveryDate;
        }

        private void ValidateInputs(string title, int copiesCount)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(copiesCount);
        }
    }
}
