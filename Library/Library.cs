using System;
using System.Collections.Generic;
using System.Linq;

namespace Librarry;

public class Library
{
    private readonly object _lockObj = new object();

    public Dictionary<int, Book> books = new Dictionary<int, Book>();
    
    private readonly IStorage _storage;

    public Library(IStorage storage)
    {
        _storage = storage;
        LoadData(); 
    }

    private void SaveData() => _storage.Save(books);

    private void LoadData() 
    {
        books = _storage.Load();
    }
    
    public bool AddBook(Book book)
    {
        if (books.TryAdd(book.ID, book))
        {
            SaveData();
            return true;
        }
        return false;
    }

    public bool RemoveBook(int id)
    {
        if (books.Remove(id))
        {
            SaveData(); 
            return true;
        }
        return false;
    }

    public IEnumerable<Book> SearchBook(string search)
    {
        return books.Values.Where(book => book.Title.Contains(search, StringComparison.OrdinalIgnoreCase) || book.Author.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
    }
    
    public IEnumerable<Book> ShowAllFreeBooks()
    { 
        return books.Values.Where(book => book.Status == BookStatus.Avaliable).ToList();
    }

    public bool BorrowBook(Book book)
    {
        lock (_lockObj)
        {
            if (book.Status == BookStatus.Avaliable)
            {
                book.Status = BookStatus.Borrowed;
                SaveData();
                return true;
            }

            return false;
        }
    }

    public bool ReturnBook(Book book)
    {
        lock (_lockObj)
        {
            if (book.Status == BookStatus.Borrowed) 
            {
                book.Status = BookStatus.Avaliable;
                SaveData(); 
                return true;
            }
            return false;  
        }
    }
}