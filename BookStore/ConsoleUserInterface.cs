using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    public class ConsoleUserInterface
    {
        private readonly ISupplierService _supplierService;

        private readonly Cart cart = new();

        public ConsoleUserInterface(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public  void CreateBook()
        {
            Console.Write("Enter the title of the book: ");
            string title = Console.ReadLine();
            Console.Write("Enter the price of the book: ");
            double price;
            while (!double.TryParse(Console.ReadLine(), out price))
            {
                Console.WriteLine("Invalid input. Please enter a valid price.");
                Console.Write("Enter the price of the book: ");
            }

            Book newBook = new(title, price);
            BookStoreDb.AddBook(newBook);

            Console.WriteLine("Book created successfully!");
        }

        public void ListBooks()
        {
            var books = BookStoreDb.GetBooks();
            if (books.Count == 0)
            {
                Console.WriteLine("No books available.");
            }
            else
            {
                Console.WriteLine("List of Books:");
                DisplayBookTable(books);
            }
        }

        public void AddToCart()
        {
            ListBooks();
            Console.Write("Enter the id of the book to add to the cart: ");
            int bookId;
            while (!int.TryParse(Console.ReadLine(), out bookId))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
                Console.Write("Enter the id of the book to add to the cart: ");
            }

            Book selectedBook = BookStoreDb.GetBook(bookId);

            if (selectedBook != null)
            {
                Console.Write("Enter the quantity of copies to buy: ");
                int quantity;
                while (!int.TryParse(Console.ReadLine(), out quantity))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                    Console.Write("Enter the quantity of copies to buy: ");
                }

                cart.AddItem(selectedBook, quantity);
                Console.WriteLine($"{selectedBook.Title} added to the cart.");
            }
            else
            {
                Console.WriteLine("Book not found in the bookstore.");
            }
        }

        public void ViewCart()
        {
            if (cart.Items.Count == 0)
            {
                Console.WriteLine("Your cart is empty.");
            }
            else
            {
                Console.WriteLine("Shopping Cart:");
                DisplayCart();
            }
        }

        private void DisplayCart()
        {
            Console.WriteLine("Cart Items:");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("| Book Title | Price      | Quantity | SubTotal |");
            Console.WriteLine("------------------------------------------------");

            foreach (var item in cart!.Items)
            {
                Console.WriteLine($"| {item.Book.Title,-10} | {item.Book.Price,-10} | {item.Quantity,-8} | {item.Subtotal,-8} |");
            }

            Console.WriteLine("------------------------------------------------");
        }

        public void Checkout()
        {

            var deliveryDate = cart.Checkout(_supplierService);
            Console.WriteLine($"Total amount to pay: ${Math.Round(cart.TotalPrice, 2)}");
            Console.WriteLine($"Your delivery is estimated to be on {deliveryDate}");
            Console.WriteLine("Thank you for shopping with us!");
        }

        public void IncreaseStock()
        {
            Console.Write("Enter the id of the book: ");
            int bookId;
            while (!int.TryParse(Console.ReadLine(), out bookId))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
                Console.Write("Enter the id of the book: ");
            }

            Book book = BookStoreDb.GetBook(bookId);
            if (book == null)
            {
                Console.WriteLine("Book not found in the bookstore.");
                return;
            }

            Console.Write("Enter the quantity of copies to add: ");
            int quantity;
            while (!int.TryParse(Console.ReadLine(), out quantity))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
                Console.Write("Enter the quantity of copies to add: ");
            }

            book.AddCopies(quantity);
            BookStoreDb.UpdateBook(book);

            Console.WriteLine("Book quantity updated successfully!");
        }

        public void Exit()
        {
            Environment.Exit(0);
        }

        private void DisplayBookTable(List<Book> books)
        {
            // Define headers
            string[] headers = { "Id", "Title", "Price", "Quantity" };

            // Display upper border
            Console.WriteLine(new string('-', 15 * headers.Length + headers.Length + 1));

            // Display header
            foreach (var header in headers)
            {
                Console.Write($"| {header,-14}");
            }
            Console.WriteLine("|");

            // Display middle border
            Console.WriteLine(new string('-', 15 * headers.Length + headers.Length + 1));

            // Display data
            foreach (var book in books)
            {
                Console.Write($"| {book.Id,-14}");
                Console.Write($"| {book.Title,-14}");
                Console.Write($"| {book.Price,-14}");
                Console.Write($"| {book.Quantity,-14}");
                Console.WriteLine("|");
            }

            // Display lower border
            Console.WriteLine(new string('-', 15 * headers.Length + headers.Length + 1));
        }

        
    }
}
