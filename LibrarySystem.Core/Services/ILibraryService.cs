using LibrarySystem.Core.DTO;

namespace LibrarySystem.Core.Services;

public interface ILibraryService
{
    int RegisterUser(string name);
    BookDto? FindBookByTitle(string title);
    void IssueBook(int bookId, int userId);
    void ReturnBook(int bookId);
}