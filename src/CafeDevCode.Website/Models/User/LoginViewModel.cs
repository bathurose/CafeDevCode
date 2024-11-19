using System.ComponentModel.DataAnnotations;
using CafeDevCode.Logic.Commands.Request;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CafeDevCode.Website.Models.User
{
    public class LoginViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Ten dang nhap khong duoc bo trong")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Mat khau khong duoc bo trong")]
        public string? Password { get; set; }
        public bool RememberPassword { get; set; }

        public string? ReturnUrl { get; set; }

        public Login ToCommand()
        {
            return new Login()
            {
                UserName = UserName,
                Password = Password,
                RememberPassword = RememberPassword
            };
        }

        public string? ErrorMessage { get; set; }
    }
}
