using LibrarySystem.Core.DTO;
using LibrarySystem.Core.Models;
using LibrarySystem.Core.Repositories;

namespace LibrarySystem.Core.Services;

public class LibraryService : ILibraryService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;

    public LibraryService(IBookRepository bookRepository, IUserRepository userRepository)
    {
        _bookRepository = bookRepository;
        _userRepository = userRepository;
    }
    
    public int RegisterUser(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));

        var user = new User { Name = name };
        _userRepository.Add(user);
        return user.Id;
    }

    public BookDto? FindBookByTitle(string title)
    {
        var book = _bookRepository.FindByTitle(title).FirstOrDefault();
        if (book == null) return null;

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Status = book.IsBorrowed ? "Taken" : "Available"
        };
    }

    public void IssueBook(int bookId, int userId)
    {
        var book = _bookRepository.GetById(bookId);
        if (book == null)
            throw new Exception("Book not found");
        if (book.IsBorrowed)
            throw new Exception("Book is already borrowed");
        
        var user = _userRepository.GetById(userId);
        if (user == null)
            throw new Exception("User not found");

        book.IsBorrowed = true;
        book.BorrowerId = userId;
        _bookRepository.Update(book);
    }

    public void ReturnBook(int bookId)
    {
        var book = _bookRepository.GetById(bookId);
        if (book == null)
            throw new Exception("Book not found");
        
        if (!book.IsBorrowed)
            throw new Exception("This  book is not borrowed");
        
        book.IsBorrowed = false;
        book.BorrowerId = null;
        _bookRepository.Update(book);
    }
}