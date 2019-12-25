using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestniZadatak.Models;

namespace TestniZadatak.Services
{
    public class UserRepository : IUserRepository
    {
        private TestDbContext _userContext;

        public UserRepository(TestDbContext userContext)
        {
            _userContext = userContext;
        }

        public bool CreateUser(User user)
        {
            user.AccountBalance = 1000;
            _userContext.AddAsync(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _userContext.Remove(user);
            return Save();
        }

        public User GetUser(int userId)
        {
            return _userContext.Users.Where(u => u.Id == userId).FirstOrDefault();
        }

        public ICollection<User> GetUsers()
        {
            return _userContext.Users.OrderBy(u => u.Id).ToList();
        }

        public bool Save()
        {
            var saved = _userContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateUser(User user)
        {
            _userContext.Update(user);
            return Save();
        }

        public bool UserExists(int userId)
        {
            return _userContext.Users.Any(u => u.Id == userId);
        }
    }
}
