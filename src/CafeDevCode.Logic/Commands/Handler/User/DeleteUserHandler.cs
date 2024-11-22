using CafeDevCode.Logic.Commands.Request;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Logic.Commands.Handler
{
    public class DeleteUserHandler : IRequestHandler<DeleteUser, BaseCommandResultWithData<User>>
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        public DeleteUserHandler(IMapper mapper, UserManager<User> userManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
        }
        public Task<BaseCommandResultWithData<User>> Handle(DeleteUser request, CancellationToken cancellationToken)
        {
            var result = new BaseCommandResultWithData<User>();
            try
            {
                var user = userManager.FindByNameAsync(request.DeleteUserName).Result;
                if (user != null)
                {
                    var deleteResult = userManager.DeleteAsync(user).Result;

                    if (deleteResult.Succeeded)
                    {
                        result.Success = true;
                    }
                    else
                    {
                        result.Messages = string.Join("-", deleteResult.Errors.Select(x => x.Description));
                    }
                }
                else {
                    result.Messages = "Can not find user to delete";
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
