using Microsoft.AspNetCore.Identity;

namespace GeekShopping.Authentication.Entities
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string Name { get; set; }
    }
}
