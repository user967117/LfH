namespace lbrry;

public class ConsoleLibraryUI
{
    private readonly Library _library;

    public ConsoleLibraryUI(Library library)
    {
        _library = library;
    }

    public void AddBook()
    {
        Console.WriteLine("Enter Title: ");
        var title = Console.ReadLine();
        
        Console.WriteLine("Enter Author: ");
        var author = Console.ReadLine();
        
        Console.WriteLine("Enter Year: ");
        var year = int.Parse(Console.ReadLine());
        
        Console.WriteLine("Enter ID: ");
        var id = int.Parse(Console.ReadLine());
        
        var newBook = new Book(title, author, year, id);

        var succes = _library.AddBook(newBook);
        if (succes)
        {
            Console.WriteLine("Book added successfully");
        }
        else
        {
            Console.WriteLine("Book could not be added");
        }
    }

    public void RemoveBook()
    {
        Console.WriteLine("Enter book ID you want to remove: ");
        var ID = int.Parse(Console.ReadLine());
        
        var succes = _library.RemoveBook(ID);
        if (succes)
        {
            Console.WriteLine("Book removed successfully");
        }
        else
        {
            Console.WriteLine("Book could not be removed");
        }
    }

    public void SearchBook()
    {
        Console.WriteLine("Enter book title or author: ");
        var input = Console.ReadLine();
        
        var result = _library.SearchBook(input).ToList();

        if (!result.Any())
        {
            Console.WriteLine("No books found");
            return;
        }

        foreach (var book in result)
        {
            Console.WriteLine($"{book.Title} | {book.Author} | {book.Year} | {book.ID} | {book.Status}");
        }
    }
    public void ShowAllFreeBook()
    {
        var books = _library.ShowAllFreeBooks();
        if(!books.Any())
        {
            Console.WriteLine("No books found");
        }
        else
        {
            foreach (var book in books)
            {
                Console.WriteLine($"{book.Title} | {book.Author} | {book.Year} | {book.ID}");
            }
        }
    }

    public void BorrowBook()
    {
        Console.WriteLine("Enter book title you want to borrow: ");
        var title = Console.ReadLine();
        
        Book book = _library.SearchBook(title).FirstOrDefault();

        if (book == null)
        {
            Console.WriteLine("Book not found");
            return;
        }
        
        bool success = _library.BorrowBook(book);

        if (success)
        {
            Console.WriteLine("Book borrowed successfully");
        }
        else
        {
            Console.WriteLine("Book could not be borrowed");
        }
    }

    public void ReturnBook()
    {
        Console.WriteLine("Enter book title you want to return: ");
        
        var title = Console.ReadLine();
        
        Book book = _library.SearchBook(title).FirstOrDefault();

        if (book == null)
        {
            Console.WriteLine("Book not found");
            return;
        }
        
        bool success = _library.ReturnBook(book);

        if (success)
        {
            Console.WriteLine("Book borrowed successfully");
        }
        else
        {
            Console.WriteLine("Book could not be borrowed");
        }
    }
}