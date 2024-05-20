using System.ComponentModel.DataAnnotations;

namespace GeekShopping.Authentication.DTOs
{
    public class TokenDTO
    {

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }
}
