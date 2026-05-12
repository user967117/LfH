using System;
using System.Collections.Generic;
using System.Linq;

namespace lbrry;

public class Library
{

    public Dictionary<int, Book> books = new Dictionary<int, Book>();
    
    public Library()
    {
        LoadData(); 
    }
    
    private void SaveData() => StorageService.Save(books);
    
    private void LoadData() 
    {
        books = StorageService.Load();
    }
    
    public void AddBook(Book book)
    {
        if (books.TryAdd(book.ID, book))
        {
            Console.WriteLine("Successfully added\n");
            SaveData(); 
        }
        else
        {
            Console.WriteLine("Failed to add: ID already exists\n");
        }
    }

    public void RemoveBook(Book book)
    {
        if (books.Remove(book.ID))
        {
            Console.WriteLine("Successfully removed\n");
            SaveData(); 
        }
        else
        {
            Console.WriteLine("Failed to remove\n");
        }
    }

    public void SearchBook(string search)
    {
        var searcedBooks = books.Values.Where(book => book.Title.Contains(search) || book.Author.Contains(search)).ToList();

        if (!searcedBooks.Any())
        {
            Console.WriteLine("No books found\n");
            return;
        }

        foreach (var book in searcedBooks)
        {
            Console.WriteLine($"Title:{book.Title} Author:{book.Author} Year:{book.Year} ID:{book.ID}");
        }
    }
    
    public void ShowAllFreeBooks()
    { 
        var freeBooks = books.Values.Where(book => book.Status).ToList();
        if (!freeBooks.Any())
        {
            Console.WriteLine("No free books found\n");
            return;
        }
        foreach (var book in freeBooks)
        {
            Console.WriteLine($"Title: {book.Title} | Author: {book.Author} | Year: {book.Year} | ID: {book.ID}");
        }
    }

    public void BorrowBook(Book book)
    {
        if (book.Status)
        {
            book.Status = false;
            Console.WriteLine($"Book '{book.Title}' successfully borrowed\n");
            SaveData(); 
        }
        else
        {
            Console.WriteLine($"Book '{book.Title}' is already borrowed\n");
        }
    }

    public void ReturnBook(Book book)
    {
        if (!book.Status) 
        {
            book.Status = true;
            Console.WriteLine($"Book '{book.Title}' successfully returned\n");
            SaveData(); 
        }
        else
        {
            Console.WriteLine($"Book '{book.Title}' is already in the library\n");
        }
    }
}