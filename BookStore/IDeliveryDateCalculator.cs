using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    public interface IDeliveryDateCalculator
    {
        DateOnly Calculate(int buisnessDaysToAdd);

        int CalculateBusinessDaysToAdd(int copiesCount);
    }
}
