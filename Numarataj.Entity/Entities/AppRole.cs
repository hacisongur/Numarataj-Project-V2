using Microsoft.AspNetCore.Identity;


namespace Numarataj.Entity.Entities
{
    public class AppRole: IdentityRole
    {
        public string FullName { get; set; } = string.Empty;
    }
    
}
