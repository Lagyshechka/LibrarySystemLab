using LibrarySystem.Core.DTO;
using Moq;
using Xunit;
using LibrarySystem.Core.Services;
using LibrarySystem.Core.Models;
using LibrarySystem.Core.Repositories;

namespace LibrarySystem.Tests;

public class LibraryServiceTests
{
    private readonly Mock<IBookRepository> _mockBookRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly LibraryService _service;

    public LibraryServiceTests()
    {
        _mockBookRepo = new Mock<IBookRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        
        _service = new LibraryService(_mockBookRepo.Object, _mockUserRepo.Object);
    }

    [Fact]
    public void RegisterUser_ValidName_CallsRepositoryAdd()
    {
        _service.RegisterUser("Aria");
        
        _mockUserRepo.Verify(r =>
            r.Add(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void RegisterUser_EmptyName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            _service.RegisterUser(string.Empty));
    }

    [Fact]
    public void FindBookByTitle_ExistingBook_ReturnsDtoWithCorrectStatus()
    {
        var books = new List<Book>
        {
            new Book { Id = 1,
                Title = "Pet Sematary",
                Author = "Steven King",
                IsBorrowed = false }
        };
        _mockBookRepo.Setup(r =>
            r.FindByTitle("Pet Sematary")).Returns(books);
        
        var result = _service.FindBookByTitle("Pet Sematary");
        
        Assert.NotNull(result);
        Assert.Equal("Pet Sematary", result.Title);
        Assert.Equal("Available", result.Status);
    }

    [Fact]
    public void IssueBook_ValidData_UpdateBookStatus()
    {
        var book = new Book { Id = 1, IsBorrowed = false };
        var user = new User { Id = 1 };
        
        _mockBookRepo.Setup(r =>
            r.GetById(1)).Returns(book);
        _mockUserRepo.Setup(r =>
            r.GetById(1)).Returns(user);
        
        _service.IssueBook(1, 1);
        
        Assert.True(book.IsBorrowed);
        Assert.Equal(1, book.BorrowerId);
        _mockBookRepo.Verify(r =>
            r.Update(book), Times.Once);
    }

    [Fact]
    public void IssueBook_AlreadyBorrowed_ThrowsException()
    {
        var book = new Book { Id = 1, IsBorrowed = true };
        _mockBookRepo.Setup(r => r.GetById(1))
            .Returns(book);

        var exception = Assert.Throws<Exception>(() =>
            _service.IssueBook(1, 1));
        Assert.Equal("Book is already borrowed", exception.Message);
    }
    
    [Fact]
    public void IssueBook_UserDoesNotExist_ThrowsException()
    {
        var book = new Book { Id = 1, IsBorrowed = false };
        _mockBookRepo.Setup(r =>
                r.GetById(1))
            .Returns(book);
        _mockUserRepo.Setup(r =>
                r.GetById(1))
            .Returns((User)null!); 

        var exception = Assert.Throws<Exception>(() =>
            _service.IssueBook(1, 1));
        Assert.Equal("User not found", exception.Message);
    }
    
    [Fact]
    public void ReturnBook_ValidBorrowedBook_UpdatesBookStatus()
    {
        var book = new Book { 
            Id = 1,
            IsBorrowed = true,
            BorrowerId = 1 
        };
        _mockBookRepo.Setup(r =>
            r.GetById(1)).Returns(book);

        _service.ReturnBook(1);

        Assert.False(book.IsBorrowed);
        Assert.Null(book.BorrowerId);
        _mockBookRepo.Verify(r =>
            r.Update(book), Times.Once);
    }
    
    [Fact]
    public void ReturnBook_BookNotBorrowed_ThrowsException()
    {
        var book = new Book
        {
            Id = 1,
            IsBorrowed = false
        };
        _mockBookRepo.Setup(r =>
            r.GetById(1)).Returns(book);

        var exception = Assert.Throws<Exception>(() =>
            _service.ReturnBook(1));
        Assert.Equal("This  book is not borrowed", exception.Message);
    }
}