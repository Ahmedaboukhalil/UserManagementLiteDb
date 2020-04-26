using System.Collections.Generic;
using UserManagementLiteDb.Models;

namespace UserManagementLiteDb.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        int Insert(User user, string password);
        bool Update(User user);
        bool Delete(int id);
    }
}
