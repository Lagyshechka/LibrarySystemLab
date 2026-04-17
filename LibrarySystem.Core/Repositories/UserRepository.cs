using LibrarySystem.Core.Models;

namespace LibrarySystem.Core.Repositories;

public class UserRepository
{
    private readonly List<User> _users = new();
    private int _nextId = 1;

    public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);

    public void Add(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
    }
}