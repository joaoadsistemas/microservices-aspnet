using System.ComponentModel.DataAnnotations;

namespace GeekShopping.Authentication.DTOs
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "User name is required")]
        [MinLength(3)]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MinLength(4)]
        [MaxLength(255)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Phone is required")]
        [MinLength(11)]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8)]
        [MaxLength(255)]
        public string Password { get; set; }

    }
}