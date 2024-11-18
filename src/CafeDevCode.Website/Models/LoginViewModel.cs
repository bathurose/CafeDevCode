using System.ComponentModel.DataAnnotations;
using CafeDevCode.Logic.Commands.Request;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CafeDevCode.Website.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Ten dang nhap khong duoc bo trong")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Mat khau khong duoc bo trong")]
        public string? Password { get; set; }
        public bool RememberPassword { get; set; }

        public string? ReturnUrl {  get; set; }

        public Logic.Commands.Request.Login ToCommand()
        {
            return new Logic.Commands.Request.Login() 
            {
                UserName = UserName,
                Password = Password,
                RememberPassword = RememberPassword
            };         
        }

        public string? ErrorMessage { get; set; }
    }
}
