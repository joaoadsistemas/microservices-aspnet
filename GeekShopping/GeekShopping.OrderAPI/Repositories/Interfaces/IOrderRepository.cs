using GeekShopping.OrderAPI.Entities;

namespace GeekShopping.OrderAPI.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(OrderHeader header);
        Task UpdateOrderPaymentStatus(long orderHeaderId, bool status);
    }
}
