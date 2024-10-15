using Microsoft.AspNetCore.Identity;

namespace HMSApi.Models.Domain
{
    public class User:IdentityUser
    {
        public string Role { get; set; }
    }
}
