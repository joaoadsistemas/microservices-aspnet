using GeekShopping.Email.Context;
using GeekShopping.Email.DTOs;
using GeekShopping.Email.Entities;
using GeekShopping.Email.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace GeekShopping.Email.Repositories
{
    public class EmailRepository : IEmailRepository
    {

        // CONTEXTO DIFERENTE UTILIZANDO DBCONTEXTOPTIONS
        private readonly DbContextOptions<SystemDbContext> _dbContext;

        public EmailRepository(DbContextOptions<SystemDbContext> dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task  SendEmailAsync(ProcessLogsDTOs message)
        {
            
            EmailLog email = new EmailLog()
            {
                Email = message.Email,
                SentDate = DateTime.Now,
                Log = $"Order - {message.OrderId} has been created successfully"
            };

            await using var _db = new SystemDbContext(_dbContext);
            _db.Emails.Add(email);
            await _db.SaveChangesAsync();

        }
    };


}
