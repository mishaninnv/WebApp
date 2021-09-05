using System;
using Logic.Context;
using Logic.Models;
using System.Linq;

namespace Logic.Repository
{
    public class UserRepository : IUserRepository
    {
        private UserContext _db;

        public UserRepository(UserContext userContext)
        {
            _db = userContext;
        }

        public void Create(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public User Read(string phone)
        {
            return _db.Users.Where(s => s.Phone == phone).FirstOrDefault();
        }

        public void Update(string phone)
        {
            var user = Read(phone);
            user.LastLogin = DateTime.Now;
            _db.SaveChanges();
        }

        public bool CheckUserExist(string phone, string password)
        {
            return _db.Users.Where(s => s.Phone == phone && s.Password == password).FirstOrDefault() != null;
        }

        public bool CheckPhoneExist(string phone)
        {
            return _db.Users.Where(s => s.Phone == phone).FirstOrDefault() != null;
        }

        public bool CheckEmailExist(string email)
        {
            return _db.Users.Where(s => s.Email == email).FirstOrDefault() != null;
        }
    }
}
