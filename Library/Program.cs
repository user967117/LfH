using Librarry;

class Program
{
    static void Main()
    {
        IStorage storage = new JsonStorageService();
        
        Library library = new Library(storage);

        ConsoleCommandsHandler libraryUI = new ConsoleCommandsHandler(library);
        while (true)
        {
            Console.WriteLine("Library menu");
            Console.WriteLine("Choose an operation");
            Console.WriteLine("1. Add book");
            Console.WriteLine("2. Remove book");
            Console.WriteLine("3. Search book by author or title");
            Console.WriteLine("4. Show all free books");
            Console.WriteLine("5. Borrow book");
            Console.WriteLine("6. Return book");
            
            var switcher = Console.ReadLine();

            switch (switcher)
            {
                case "1":
                    libraryUI.AddBook();
                    break;
                case "2":
                    libraryUI.RemoveBook();
                    break;
                case "3":
                    libraryUI.SearchBook();
                    break;
                case "4":
                    libraryUI.ShowAllFreeBook();
                    break;
                case "5":
                    libraryUI.BorrowBook();
                    break;
                case "6":
                    libraryUI.ReturnBook();
                    break;
            }
        }
    }
}