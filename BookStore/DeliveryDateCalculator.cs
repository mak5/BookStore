using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    public class DeliveryDateCalculator : IDeliveryDateCalculator
    {
        public DateOnly Calculate(int businessDaysToAdd)
        {
            var deliveryDate = DateOnly.FromDateTime(DateTime.Now);
            var addedDays = 0;

            while (addedDays < businessDaysToAdd)
            {
                deliveryDate = deliveryDate.AddDays(1);

                if (IsBusinessDay(deliveryDate))
                {
                    addedDays++;
                }
            }

            return deliveryDate;
        }

        public int CalculateBusinessDaysToAdd(int copiesCount)
        {
            if (copiesCount < 5)
            {
                return 2;
            }
            else if (copiesCount < 10)
            {
                return 3;
            }
            else
            {
                return 5;
            }
        }

        private bool IsBusinessDay(DateOnly date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }
    }
}
