using Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace Logic.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
    }
}
