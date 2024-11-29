using CafeDevCode.Logic.Commands.Request;
using CafeDevCode.Ultils.Global;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Logic.Commands.Handler
{
    public class ResetPasswordHandler : IRequestHandler<ResetPassword, BaseCommandResult>
    {
        private readonly UserManager<User> userManager;

        public ResetPasswordHandler(UserManager<User> userManager) {
            this.userManager = userManager;
        }
        public Task<BaseCommandResult> Handle(ResetPassword request, CancellationToken cancellationToken)
        {
            var result = new BaseCommandResult();
            try
            {
                var user = userManager.FindByNameAsync(request.ResetUserName).Result;
                if (user != null)
                {
                    var resetToken = userManager.GeneratePasswordResetTokenAsync(user).Result;
                    var resetPassword = userManager.ResetPasswordAsync(user, resetToken, request.NewPassword).Result;

                    if (resetPassword.Succeeded)
                    {
                        result.Success = true;
                        result.Messages = AppGlobal.DefaultSuccessMessage;
                    }
                    else {
                        result.Messages = string.Join("-", resetPassword.Errors.Select(x => x.Description));
                    }

                }
                else {
                   ;
                    result.Messages = "Không tìm thấy người dùng";
                }
            }
            catch (Exception ex)
            {
            
                result.Messages = ex.Message;
            }

            return Task.FromResult(result);
        }
    }
}
