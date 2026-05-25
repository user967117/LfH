using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace Librarry;

public class Library
{
    private readonly Lock _lockObj = new Lock();

    public ConcurrentDictionary<int, Book> books = new ConcurrentDictionary<int, Book>();
    
    private readonly IStorage _storage;

    public Library(IStorage storage)
    {
        _storage = storage;
        LoadData(); 
    }

    private void SaveData() => _storage.Save(books.ToDictionary(b => b.Key, b => b.Value));

    private void LoadData() 
    {
        var loadedData = _storage.Load();
        books = new ConcurrentDictionary<int, Book>(loadedData);
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
        if (books.TryRemove(id, out _))
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