using AutoMapper;
using CafeDevCode.Logic.Commands.Request;
using CafeDevCode.Logic.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Logic.Commands.Handler
{
    public class CreateUserHandler : IRequestHandler<CreateUser, BaseCommandResultWithData<User>>
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
   
        public CreateUserHandler(IMapper mapper, UserManager<User> userManager) 
        {
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public Task<BaseCommandResultWithData<User>> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            var result = new BaseCommandResultWithData<User>();
            try
            {
                var user = mapper.Map<UserSummaryModel>(request);

                var createResult = userManager.CreateAsync(user);
                if (createResult.Result.Succeeded)
                {
                    result.Success = true;
                    result.Data = user;
                }
                else {
                    result.Success = false;
                    result.Messages = string.Join("-",createResult.Result.Errors.Select(x => x.Description));
                }

            }
            catch (Exception e)
            {
                result.Success = false;
                result.Messages = e.Message;
            }

            return Task.FromResult(result);
           

        }
    }
}


