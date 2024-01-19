using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    public class Book
    {
        public Book(string title, double price)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
            Title = title;
            Price = price;
        }

        public int Id { get; set; }

        public string Title { get; }

        public double Price { get; }

        public int Quantity { get; private set; }

        public void AddCopies(int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            Quantity += quantity;
        }
        
        public void RemoveCopies(int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            Quantity -= quantity;

            if (Quantity < 0)
            {
                Quantity = 0;
            }
        }
    }
}
