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
    public class UpdateAuthorHandler : IRequestHandler<UpdateAuthor, BaseCommandResultWithData<Database.Entities.Author>>
    {
        private readonly IMapper mapper;
        private readonly AppDatabase database;

        public UpdateAuthorHandler(IMapper mapper, AppDatabase database) 
        {
            this.mapper = mapper;
            this.database = database;
        }

        public Task<BaseCommandResultWithData<Database.Entities.Author>> Handle(UpdateAuthor request, CancellationToken cancellationToken)
        {
            var result = new BaseCommandResultWithData<Database.Entities.Author>();
            try
            {
                var author = database.Authors.FirstOrDefault(x => x.Id == request.Id);
                if (author != null)
                {
                    mapper.Map(request, author);

                    request.SetUpdateInfo(request.UserName ?? string.Empty, AppGlobal.SysDateTime);

                    database.Update(author);

                    database.SaveChanges();

                    result.Success = true;

                    result.Data=author;
                }
                else 
                {
                    result.Message = $"Khong tim thay tac gia voi ma id {request.Id}";
                }

            }
            catch (Exception ex) 
            { 
                result.Message= ex.Message;
            }
            return Task.FromResult(result);
        }
    }

}
