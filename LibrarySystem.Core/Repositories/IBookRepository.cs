using LibrarySystem.Core.DTO;

namespace LibrarySystem.Core.Repositories;

public interface IBookRepository
{
    Book? GetById(int id);
    IEnumerable<Book> GetAll();
    IEnumerable<Book> FindByTitle(string title);
    void Add(Book book);
    void Update(Book book);
}