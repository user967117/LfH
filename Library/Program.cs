using lbrry;

class Program
{
    static void Main()
    {
        Library library = new Library();
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
                    Console.WriteLine("Enter author");
                    var author = Console.ReadLine();
                    
                    Console.WriteLine("Enter title");
                    var title = Console.ReadLine();
                    
                    Console.WriteLine("Enter ID");
                    var ID = int.Parse(Console.ReadLine());
                    
                    Console.WriteLine("Enter year");
                    var year = int.Parse(Console.ReadLine());
                    
                    library.AddBook(new Book(title, author, year, ID, true));
                    break;
                case "2":
                    Console.WriteLine("Enter book id you want to remove");
                    int removeID = int.Parse(Console.ReadLine());
                    var bookToRemove = library.books.Values.FirstOrDefault(b => b.ID == removeID);
                    if (bookToRemove != null)
                    {
                        library.RemoveBook(bookToRemove);
                    }
                    else
                    {
                        Console.WriteLine("Book not found\n");
                    }
                    break;
                case "3":
                    Console.WriteLine("Enter book author or title you wanna search");
                    var search = Console.ReadLine();
                    library.SearchBook(search);
                    break;
                case "4":
                    Console.WriteLine("Free books");
                        
                    library.ShowAllFreeBooks();
                    break;
                case "5":
                    Console.WriteLine("Enter book title you want borrow");
                    string borrowBook = Console.ReadLine();
                    var bookToBorrow = library.books.Values.FirstOrDefault(b => b.Title == borrowBook);
                    
                    library.BorrowBook(bookToBorrow);
                    break;
                case "6":
                    Console.WriteLine("Enter book title you want borrow");
                    string returnBook = Console.ReadLine();
                    var bookToReturn = library.books.Values.FirstOrDefault(b => b.Title == returnBook);
                    
                    library.BorrowBook(bookToReturn);
                    break;
            }
        }
    }
}