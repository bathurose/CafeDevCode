using System.ComponentModel.DataAnnotations;

namespace CafeDevCode.Website.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Ten dang nhap khong duoc bo trong")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Mat khau khong duoc bo trong")]
        public string? Password { get; set; }
        public bool RememberPassword { get; set; }
    }
}
