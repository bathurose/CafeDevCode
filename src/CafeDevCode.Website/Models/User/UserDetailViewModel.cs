using CafeDevCode.Logic.Commands.Request;
using System.ComponentModel.DataAnnotations;

namespace CafeDevCode.Website.Models.User
{
    public class UserDetailViewModel : BaseViewModel
    {
        [Required(ErrorMessage ="Tài khoản không được bỏ trống")]
        public string? DetailUserName { get; set;}
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber  { get; set; }
        public string? AuthorId {  get; set; }
        public CreateUser ToCreateCommand()
        {
            return new CreateUser {
                CreateUserName = this.DetailUserName ?? string.Empty,
                PhoneNumber = this.PhoneNumber,
                AuthorId = this.AuthorId,
                Email = this.Email,
                Password = this.Password
            };
        }

        public UpdateUser ToUpdateCommand()
        {
            return new UpdateUser
            {
                UpdateUserName = this.DetailUserName ?? string.Empty,
                PhoneNumber = this.PhoneNumber,
                AuthorId = this.AuthorId,
                Email= this.Email
            };
        }
    }
}
