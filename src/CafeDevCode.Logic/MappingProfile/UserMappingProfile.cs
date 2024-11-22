using CafeDevCode.Logic.Commands.Request;
using CafeDevCode.Logic.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Logic.MappingProfile
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() {
            CreateMap<CreateUser, User>()
                .ForMember(x => x.UserName, y => y.MapFrom(s => s.CreateUserName));
         
            CreateMap<UserSummaryModel, User>().ReverseMap();

            CreateMap<UserDetailModel, User>().ReverseMap();

        }
    }
}
