using BookStore;

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
            ConsoleUserInterface.CreateBook();
            break;
        case "2":
            ConsoleUserInterface.ListBooks();
            break;
        case "3":
            ConsoleUserInterface.AddToCart();
            break;
        case "4":
            ConsoleUserInterface.ViewCart();
            break;
        case "5":
            ConsoleUserInterface.Checkout();
            break;
        case "6":
            ConsoleUserInterface.IncreaseStock();
            break;
        case "7":
            ConsoleUserInterface.Exit();
            break;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }

    Console.WriteLine(); // Adding a newline for better readability
}