using LibrarySystem.Core.Models;

namespace LibrarySystem.Core.Repositories;

public interface IUserRepository
{
    User? GetById(int id);
    void Add(User user);
}