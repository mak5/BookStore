using BookStore;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddScoped<ISupplierService, SupplierService>()
    .AddScoped<IDeliveryDateCalculator, DeliveryDateCalculator>()
    .AddScoped<IDateTimeProvider, DateTimeProvider>()
    .BuildServiceProvider();

var supplierService = serviceProvider.GetService<ISupplierService>();
var Ui = new ConsoleUserInterface(supplierService);

while (true)
{
    Console.WriteLine("Bookstore Menu:");
    Console.WriteLine("1. Create a Book");
    Console.WriteLine("2. List Books");
    Console.WriteLine("3. Add Book to Cart");
    Console.WriteLine("4. View Cart");
    Console.WriteLine("5. Checkout");
    Console.WriteLine("6. Increase book stock");
    Console.WriteLine("7. Exit");

    Console.Write("Enter your choice: ");
    string choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            Ui.CreateBook();
            break;
        case "2":
            Ui.ListBooks();
            break;
        case "3":
            Ui.AddToCart();
            break;
        case "4":
            Ui.ViewCart();
            break;
        case "5":
            Ui.Checkout();
            break;
        case "6":
            Ui.IncreaseStock();
            break;
        case "7":
            Ui.Exit();
            break;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }

    Console.WriteLine(); // Adding a newline for better readability
}