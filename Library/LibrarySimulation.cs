namespace Librarry;

public class LibrarySimulation
{
    public static async Task RunMassSimulation(Library library, int totalVisitors)
    {
        Task[] visitors = new Task[totalVisitors];
        
        Console.WriteLine("\nStarting simulation");

        for (int i = 0; i < totalVisitors; i++)
        {
            int visitorID = i + 1;

            visitors[i] = Task.Run(async () =>
            {
                await Task.Delay(Random.Shared.Next(100, 500));

                List<Book> availibleBooks = library.ShowAllFreeBooks().ToList();

                if (availibleBooks.Count == 0)
                {
                    Console.WriteLine("No books found");
                    return;
                }

                int randomIndex = Random.Shared.Next(0, availibleBooks.Count);
                Book chosenBook = availibleBooks[randomIndex];

                if (library.BorrowBook(chosenBook))
                {
                    Console.WriteLine($"{chosenBook.Title} is borrowed by {visitorID} visitor");
                    
                    await Task.Delay(Random.Shared.Next(1000, 3000));
                    
                    library.ReturnBook(chosenBook);
                    Console.WriteLine($"{chosenBook.Title} is returned by {visitorID} visitor");
                }
                else
                {
                    Console.WriteLine($"{chosenBook.Title} is not borrowed by {visitorID} visitor, cause book was already borrowed");
                }
            });
        }
        await Task.WhenAll(visitors);
        
        Console.WriteLine("Simulation finished\n");
    }
}