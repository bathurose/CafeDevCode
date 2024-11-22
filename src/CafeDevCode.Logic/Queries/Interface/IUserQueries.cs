using CafeDevCode.Logic.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Logic.Queries.Interface
{
    public interface IUserQueries
    {
        UserDetailModel GetDetail(string username);
        Task<UserDetailModel> GetDetailAsync(string username);
        List<UserSummaryModel> GetAll();
        Task<List<UserSummaryModel>> GetAllAsync();
        BasePagingData<UserSummaryModel> GetPaging(BaseQuery query);
        Task<BasePagingData<UserSummaryModel>> GetPagingAsync(BaseQuery query);
        bool IsExistUserName (string username);




    }
}
