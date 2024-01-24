using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{ 
    public class Cart
    {
        public List<CartItem> Items { get; private set; }

        public string CustomerName { get; }

        public bool Completed { get; private set; }

        public Cart()
        {
            Items = [];
            CustomerName = $"Customer_{Guid.NewGuid()}";
        }

        public Cart(string customerName)
        {
            Items = [];
            CustomerName = customerName ?? $"Customer_{Guid.NewGuid()}";
        }

        public void AddItem(Book book, int quantity)
        {
            ValidityCheck();

            CartItem? existingItem = Items.Find(item => item.Book.Id == book.Id);

            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                CartItem newItem = new(book, quantity);
                Items.Add(newItem);
            }
        }



        public void RemoveItem(Book book, int quantity)
        {
            ValidityCheck();

            CartItem? existingItem = Items.Find(item => item.Book.Id == book.Id);

            if (existingItem != null)
            {
                if (existingItem.Quantity > quantity)
                {
                    existingItem.DecreaseQuantity(quantity);
                }
                else
                {
                    Items.Remove(existingItem);
                }
            }
        }

        public DateOnly Checkout(ISupplierService supplierService)
        {
            DateOnly deliveryDate = DateOnly.FromDateTime(DateTime.Now);
            foreach (var item in Items)
            {

                if (item.Quantity > item.Book.Quantity)
                {
                    var expectedOrderDate = supplierService.OrderCopies(item.Book.Title, item.Quantity - item.Book.Quantity);
                    if ( expectedOrderDate > deliveryDate)
                    {
                        deliveryDate = expectedOrderDate;
                    }
                }

                item.Book.RemoveCopies(item.Quantity);
            }

            Completed = true;

            return deliveryDate;
        }

        public double TotalPrice => Items.Sum(item => item.Subtotal);

        private void ValidityCheck()
        {
            if (Completed)
            {
                throw new InvalidOperationException("Cart is completed no changes are allowed!");
            }
        }
    }

    public class CartItem
    {
        public Book Book { get; }
        public int Quantity { get; private set; }

        public CartItem(Book book, int quantity)
        {
            ArgumentNullException.ThrowIfNull(book);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            Book = book;
            Quantity = quantity;
        }

        public void IncreaseQuantity(int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            Quantity += quantity;
        }

        public void DecreaseQuantity(int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            Quantity -= quantity;
        }

        public double Subtotal
        {
            get { return Book.Price * Quantity; }
        }
    }
}
