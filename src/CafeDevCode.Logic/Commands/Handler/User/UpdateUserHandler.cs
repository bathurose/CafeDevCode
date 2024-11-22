using CafeDevCode.Logic.Commands.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Logic.Commands.Handler
{
    public class UpdateUserHandler : IRequestHandler<UpdateUser, BaseCommandResultWithData<User>>
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        public UpdateUserHandler(IMapper mapper, UserManager<User> userManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public Task<BaseCommandResultWithData<User>> Handle(UpdateUser request, CancellationToken cancellationToken)
        {
           var result = new BaseCommandResultWithData<User>();

            try
            {
                var user = userManager.FindByNameAsync(request.UpdateUserName).Result;

                if (user != null)
                {
                    user.Email = request.Email;
                    user.AuthorId = request.AuthorId;
                    user.PhoneNumber = request.PhoneNumber;

                    var updateResult = userManager.UpdateAsync(user);

                    if (updateResult.Result.Succeeded)
                    {
                        result.Success = true;
                        result.Data = user;
                    }
                    else
                    {

                        result.Messages = string.Join("-", updateResult.Result.Errors.Select(x => x.Description));
                    }

                }
                else {
                    result.Messages = "Can not find the user"; 
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
