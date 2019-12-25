using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestniZadatak.Models;

namespace TestniZadatak.Services
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int userId);

        bool UserExists(int userId);

        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();

    }
}
