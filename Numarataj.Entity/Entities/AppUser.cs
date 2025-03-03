
using Microsoft.AspNetCore.Identity;

namespace Numarataj.Entity.Entities
{
    public class AppUser: IdentityUser
    {
        public string? FullName { get; set; }
    }
}
