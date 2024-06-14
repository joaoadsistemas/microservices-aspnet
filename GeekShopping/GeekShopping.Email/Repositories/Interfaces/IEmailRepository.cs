using GeekShopping.Email.DTOs;

namespace GeekShopping.Email.Repositories.Interfaces
{
    public interface IEmailRepository
    {

        Task SendEmailAsync(ProcessLogsDTOs message);

    }
}
