using System;
using LibrarySystem.Core.Services;

namespace LibrarySystem.Core.Controllers;

public class LibraryController
{
    private readonly ILibraryService _libraryService;

    public LibraryController(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    public void IssueBookEndpoint(int bookId, int userId)
    {
        try
        {
            _libraryService.IssueBook(bookId, userId);
            Console.WriteLine("Successfully given the book to the reader");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void ReturnBookEndpoint(int bookId)
    {
        try
        {
            _libraryService.ReturnBook(bookId);
            Console.WriteLine("Successfully returned the book to the library");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}