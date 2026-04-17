using LibrarySystem.Core.DTO;

namespace LibrarySystem.Core.Repositories;

public class BookRepository
{
    private readonly List<Book> _books = new();
    private int _nextId = 1;

    public Book? GetById(int id) => _books.FirstOrDefault(b => b.Id == id);
    
    public IEnumerable<Book> GetAll() => _books;
    
    public IEnumerable<Book> FindByTitle(string title) => 
        _books.Where(b => b.Title.Contains(title, System.StringComparison.OrdinalIgnoreCase));

    public void Add(Book book)
    {
        book.Id = _nextId++;
        _books.Add(book);
    }

    public void Update(Book book)
    {
        var existing = GetById(book.Id);
        if (existing != null)
        {
            existing.IsBorrowed = book.IsBorrowed;
            existing.BorrowerId = book.BorrowerId;;
        }
    }
}