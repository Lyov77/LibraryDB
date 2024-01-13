namespace LibraryDB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=***********;Initial Catalog=BookShop;Integrated Security=True;";

            Library library = new Library(connectionString);

            while (true)
            {
                Console.WriteLine("\nMenu:\n1 - Add.\n2 - Find.\n3 - Show all.\n4 - Delete.\n5 - Exit.");

                string? input = Console.ReadLine();

                if (int.TryParse(input, out int number))
                {
                    switch (number)
                    {
                        case 1:
                            Console.WriteLine("Adding new book.");
                            Console.Write("Title: ");
                            string title = Console.ReadLine();
                            Console.Write("Author: ");
                            string author = Console.ReadLine();
                            library.AddBookAndAuthor(title, author);
                            break;

                        case 2:
                            Console.WriteLine("1 - Find by title:\n2 - Find by author:");

                            string? findInput = Console.ReadLine();

                            if (int.TryParse(findInput, out int findNumber))
                            {

                                switch (findNumber)
                                {
                                    case 1:
                                        Console.Write("Please enter book title:");
                                        string title2 = Console.ReadLine();
                                        library.FindAndDisplayBook(title2);
                                        break;

                                    case 2:
                                        Console.Write("Please enter Author's name:");
                                        string authorName = Console.ReadLine();
                                        library.FindByAuthor(authorName);
                                        break;

                                    default:
                                        Console.WriteLine("Invalid input. Please enter a valid number.");
                                        break;
                                }
                            }
                            break;

                        case 3:
                            library.ShowAllBooksAndAuthors();
                            break;

                        case 4:
                            Console.WriteLine("Deleting a book.");
                            Console.Write("Title: ");
                            string title3 = Console.ReadLine();
                            library.DeleteBook(title3);
                            break;

                        case 5:
                            Console.WriteLine("Exitig");
                            return;

                        default:
                            Console.WriteLine("Invalid input. Please enter a valid number.");
                            break;
                    }
                }
            }
        }
    }
}