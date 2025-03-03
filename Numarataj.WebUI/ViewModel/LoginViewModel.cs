using System.ComponentModel.DataAnnotations;

namespace Numarataj.WebUI.ViewModel
{
    public class LoginViewModel
    {
        [EmailAddress]
        public String Email { get; set; } = null!;
        [DataType(DataType.Password)]
        public String Password { get; set; } = null!;
        public bool RememberMe { get; set; } = true;
    }
}
