namespace LibrarySystem.Core.DTO;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}