using CafeDevCode.Logic.Queries.Interface;
using CafeDevCode.Logic.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Logic.Queries.Implement
{
    public class UserQueries : IUserQueries
    {
        private readonly AppDatabase database;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public UserQueries(AppDatabase database, IMapper mapper, UserManager<User> userManager) {
            this.database = database;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        public List<UserSummaryModel> GetAll()
        {
           return userManager.Users
                .Select(x => mapper.Map<UserSummaryModel>(x))
                .ToList();
        }

        public Task<List<UserSummaryModel>> GetAllAsync()
        {
            return Task.Run(() => userManager.Users
                .Select(x => mapper.Map<UserSummaryModel>(x))
                .ToListAsync());
        }

        public UserDetailModel GetDetail(string username)
        {
            var user = userManager.FindByNameAsync(username).Result;

            return mapper.Map<UserDetailModel>(user);   

        }

        public Task<UserDetailModel> GetDetailAsync(string username)
        {
            var user = userManager.FindByNameAsync(username).Result;

            UserDetailModel? result = null;
            if (user != null)
            {
                result = mapper.Map<UserDetailModel>(user);
            }
            return Task.FromResult(result);
        }

        public BasePagingData<UserSummaryModel> GetPaging(BaseQuery query)
        {
            var users = userManager.Users
                .Where(x => x.UserName.Contains(query.KeyWords ?? string.Empty) ||
                            x.PhoneNumber.Contains(query.KeyWords ?? string.Empty) ||
                            x.Email.Contains(query.KeyWords ?? string.Empty))
                .Skip(((query.PageIndex - 1) * query.PageSize) ?? 0).Take((query.PageSize * query.PageIndex) ?? 20)
                .Select(x => mapper.Map<UserSummaryModel>(x))
                .ToList();

            var userCount = userManager.Users.Count();

            return new BasePagingData<UserSummaryModel>
            {
                Items = users,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                TotalItem = userCount,
                TotalPage = (int)Math.Ceiling((double)userCount / (query.PageSize ?? 20)) 
            };
        }

        public Task<BasePagingData<UserSummaryModel>> GetPagingAsync(BaseQuery query)
        {
            var users = userManager.Users
                .Where(x => x.UserName.Contains(query.KeyWords ?? string.Empty) ||
                            x.PhoneNumber.Contains(query.KeyWords ?? string.Empty) ||
                            x.Email.Contains(query.KeyWords ?? string.Empty))
                .Skip(((query.PageIndex - 1) * query.PageSize) ?? 0).Take((query.PageSize * query.PageIndex) ?? 20)
                .Select(x => mapper.Map<UserSummaryModel>(x))
                .ToList();

            var userCount = userManager.Users.Count();

            return Task.FromResult(new BasePagingData<UserSummaryModel>
            {
                Items = users,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                TotalItem = userCount,
                TotalPage = (int)Math.Ceiling((double)userCount / (query.PageSize ?? 20))
            });
        }

        public bool IsExistUserName(string username)
        {
            if (string.IsNullOrEmpty(username))
            { 
                return false; 
            }
        
            var result = userManager.FindByNameAsync(username).Result;
            return result != null ? true : false ;
        }
    }
}
