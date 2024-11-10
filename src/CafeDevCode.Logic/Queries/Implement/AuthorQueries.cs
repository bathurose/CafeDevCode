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
            throw new NotImplementedException();
        }

        public BasePagingData<AuthorSummaryModel> GetPaging(BaseQuery query)
        {
            var result = database.Authors
                .Where( x => x.FullName!.Contains(query.KeyWords ?? string.Empty || ))
        }

        public Task<BasePagingData<AuthorSummaryModel>> GetPagingAsync(BaseQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
