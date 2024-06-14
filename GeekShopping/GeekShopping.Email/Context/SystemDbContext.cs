using GeekShopping.Email.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Context
{
    public class SystemDbContext : DbContext
    {
        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options) { }

        public DbSet<EmailLog> Emails { get; set; }


    }
}