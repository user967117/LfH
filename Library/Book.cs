namespace lbrry;

public class Book
{
    
    public Book(){}
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public int ID  { get; set; }
    public bool Status { get; set; }
    
    public Book(string title, string author, int year, int id, bool status)
    {
        Title = title;
        Author = author;
        Year = year;
        ID = id;
        Status = status;
    }
    
}