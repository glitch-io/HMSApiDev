using System.ComponentModel.DataAnnotations;

namespace HMSApi.Models.DTO
{
    public class RegisterRequestDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
