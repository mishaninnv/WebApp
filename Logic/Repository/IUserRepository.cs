using Logic.Models;

namespace Logic.Repository
{
    public interface IUserRepository
    {
        public void Create(User user);
        public User Read(string phone);
        public void Update(string phone);
        public bool CheckUserExist(string phone, string password);
        public bool CheckPhoneExist(string phone);
        public bool CheckEmailExist(string email);
    }
}
