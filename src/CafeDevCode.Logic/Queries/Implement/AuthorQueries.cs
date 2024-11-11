using AutoMapper;
using CafeDevCode.Common.Shared.Model;
using CafeDevCode.Database;
using CafeDevCode.Logic.Queries.Interface;
using CafeDevCode.Login.Shared.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Logic.Queries.Implement
{
    public class AuthorQueires : IAuthorQueries
    {
        private readonly AppDatabase database;
        private readonly IMapper mapper;

        public AuthorQueires(AppDatabase database, IMapper mapper) {
            this.database = database;
            this.mapper = mapper;
        }  
        public List<AuthorSummaryModel> GetAll()
        {
            return database.Authors
                .Select(x => mapper.Map<AuthorSummaryModel>(x))
                .ToList();            
        }

        public Task<List<AuthorSummaryModel>> GetAllAsync()
        {
            return Task.Run(() => database.Authors
                .Select(x => mapper.Map<AuthorSummaryModel>(x))
                .ToListAsync());
        }

        public AuthorDetailModel? GetDetail(int id)
        {
            var author = database.Authors.FirstOrDefault(x => x.Id == id);
            if (author != null)
            {
                var result = mapper.Map<AuthorDetailModel>(author);
                var posts = database.Post.Where(x => x.AuthorId == id);
                result.Posts = posts.ToList();
                return result;
            }
            return null;
        }

        public Task<AuthorDetailModel?> GetDetailAsync(int id)
        {
            var author = database.Authors.FirstOrDefault(x => x.Id == id);

            AuthorDetailModel? detailModel = null;
        
            if (author != null)
            {
                detailModel = mapper.Map<AuthorDetailModel>(author);
                var posts = database.Post.Where(x => x.AuthorId == id);
                detailModel.Posts = posts.ToList();
               
            }
            return Task.FromResult(detailModel);
        }

        public BasePagingData<AuthorSummaryModel> GetPaging(BaseQuery query)
        {
            var result = database.Authors
                .Where(x => x.FullName!.Contains(query.KeyWords ?? string.Empty) ||
                            x.ShortName!.Contains(query.KeyWords ?? string.Empty) ||
                             x.Phone!.Contains(query.KeyWords ?? string.Empty) ||
                            x.Email!.Contains(query.KeyWords ?? string.Empty))
                .Skip(((query.PageIndex-1) * query.PageSize) ?? 0).Take((query.PageSize*query.PageIndex) ?? 0)
                .Select(x => mapper.Map<AuthorSummaryModel>(x))
                .ToList();
            var resultcount = database.Authors.Count();
            return new BasePagingData<AuthorSummaryModel>()
            {
                Items = result,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                TotalItem = resultcount,
                TotalPage = (int)Math.Ceiling((double)resultcount/(query.PageSize ?? 20))
            }; 
        }

        public Task<BasePagingData<AuthorSummaryModel>> GetPagingAsync(BaseQuery query)
        {
            var result = database.Authors
                .Where(x => x.FullName!.Contains(query.KeyWords ?? string.Empty) ||
                            x.ShortName!.Contains(query.KeyWords ?? string.Empty) ||
                             x.Phone!.Contains(query.KeyWords ?? string.Empty) ||
                            x.Email!.Contains(query.KeyWords ?? string.Empty))
                .Skip(((query.PageIndex-1) * query.PageSize) ?? 0).Take((query.PageSize*query.PageIndex) ?? 0)
                .Select(x => mapper.Map<AuthorSummaryModel>(x))
                .ToList();
            var resultcount = database.Authors.Count();

            return Task.FromResult(new BasePagingData<AuthorSummaryModel>()
            {
                Items = result,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                TotalItem = resultcount,
                TotalPage = (int)Math.Ceiling((double)resultcount/(query.PageSize ?? 20))
            });
        }
    }
}
