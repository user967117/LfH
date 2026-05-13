using System;
using System.Collections.Generic;
using System.Linq;

namespace lbrry;

public class Library
{

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
        return books.Values.Where(book => book.Title.Contains(search) || book.Author.Contains(search)).ToList();
    }
    
    public IEnumerable<Book> ShowAllFreeBooks()
    { 
        return books.Values.Where(book => book.Status == BookStatus.Avaliable).ToList();
    }

    public bool BorrowBook(Book book)
    {
        if (book.Status == BookStatus.Avaliable)
        {
            book.Status = BookStatus.Borrowed;
            SaveData(); 
            return  true;
        }
        return  false;
    }

    public bool ReturnBook(Book book)
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