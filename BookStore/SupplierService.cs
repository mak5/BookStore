using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    public class SupplierService : ISupplierService
    {
        public DateOnly OrderCopies(string title, int copiesCount)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(copiesCount);

            int businessDaysToAdd;

            if (copiesCount < 5)
            {
                businessDaysToAdd = 2;
            }
            
            else if (copiesCount < 10)
            {
                businessDaysToAdd = 3;
            }

            else
            {
                businessDaysToAdd = 5;
            }

            var deliveryDate = DateOnly.FromDateTime(DateTime.Now);

            var addedDays = 0;

            while (addedDays < businessDaysToAdd)
            {
                deliveryDate = deliveryDate.AddDays(1);

                if (deliveryDate.DayOfWeek != DayOfWeek.Saturday && deliveryDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    addedDays++;
                }
            }

            return deliveryDate;
        }

    }
}
