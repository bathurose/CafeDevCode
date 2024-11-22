using AutoMapper;
using CafeDevCode.Database;
using CafeDevCode.Logic.Commands.Request;
using CafeDevCode.Ultils.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Logic.Commands.Handler
{
    public class CreateAuthorHandler
        : IRequestHandler<CreateAuthor, BaseCommandResultWithData<Database.Entities.Author>>
    {
        private readonly AppDatabase database;

        public IMapper mapper { get; }

        public CreateAuthorHandler(IMapper mapper, AppDatabase database) {
            this.database = database;
            this.mapper = mapper;
        }

      
        public Task<BaseCommandResultWithData<Database.Entities.Author>> Handle(CreateAuthor request, 
            CancellationToken cancellationToken)
        {
            var result = new BaseCommandResultWithData<Database.Entities.Author>();
            try
            {
                var author = mapper.Map<Database.Entities.Author>(request);

                database.Authors.Add(author);

                author.SetCreateInfo(request.UserName ?? string.Empty, AppGlobal.SysDateTime);

                database.SaveChanges();

                result.Success = true;

                result.Data = author;

            }
            catch (Exception ex) 
            {
                result.Messages = ex.Message;
            }
            return Task.FromResult(result);
                
            
        }

        
    }
}
