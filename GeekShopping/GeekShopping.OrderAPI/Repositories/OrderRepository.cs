using GeekShopping.OrderAPI.Context;
using GeekShopping.OrderAPI.Entities;
using GeekShopping.OrderAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        // CONTEXTO DIFERENTE UTILIZANDO DBCONTEXTOPTIONS
        private readonly DbContextOptions<SystemDbContext> _dbContext;

        public OrderRepository(DbContextOptions<SystemDbContext> dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<bool> AddOrder(OrderHeader header)
        {
            if (header == null) return false;
            await using var _db = new SystemDbContext(_dbContext);
            _db.Headers.Add(header);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool status)
        {
            await using var _db = new SystemDbContext(_dbContext);
            var header = await _db.Headers.FirstOrDefaultAsync(o => o.Id == orderHeaderId);
            if (header != null)
            {
                header.PaymentStatus = status;
                await _db.SaveChangesAsync();
            }

        }
    }
}
