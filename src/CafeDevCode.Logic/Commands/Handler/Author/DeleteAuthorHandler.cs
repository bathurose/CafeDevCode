global using AutoMapper;
global using CafeDevCode.Database;
using CafeDevCode.Logic.Commands.Request;
using CafeDevCode.Ultils.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Logic.Commands.Handler
{
    public class DeleteAuthorHandler : IRequestHandler<DeleteAuthor, BaseCommandResult>
    {
        private readonly IMapper mapper;

        public AppDatabase database { get; }

        public DeleteAuthorHandler(IMapper mapper , AppDatabase database) 
        {
            this.mapper = mapper;
            this.database = database;
        }
        public Task<BaseCommandResult> Handle(DeleteAuthor request, CancellationToken cancellationToken)
        {
            var result = new BaseCommandResult();
            try
            {
                var author = database.Authors.FirstOrDefault(x => x.Id == request.Id);
                if (author != null)
                {
                    author.MarkAsDelete(request.UserName ?? string.Empty, AppGlobal.SysDateTime);

                    database.Update(author);

                    database.SaveChanges();

                    result.Success = true;
                }
                else
                {
                    result.Message = $"Khong tim thay tac gia voi ma id {request.Id}";
                }

            }
            catch(Exception ex)
            {
                result.Message = ex.Message;
            }
            return Task.FromResult(result);
        }
    }
}
