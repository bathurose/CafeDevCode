using CafeDevCode.Common.Shared.Model;
using CafeDevCode.Database.Entities;
using CafeDevCode.Logic.Commands.Request;
using CafeDevCode.Logic.Queries.Interface;
using CafeDevCode.Logic.Shared.Model;
using CafeDevCode.Ultils.Extensions;
using CafeDevCode.Ultils.Global;
using CafeDevCode.Website.Models.Author;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CafeDevCode.Website.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuthorQueries authorQueries;

        public AuthorController(IMediator mediator, IAuthorQueries authorQueries) {
            this.mediator = mediator;
            this.authorQueries = authorQueries;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int Id)
        {
            var model = new AuthorDetailModel();

            if (Id > 0)
            {
                model = authorQueries.GetDetail(Id);
            }

            return View(model);
        }

        public async Task<IActionResult> SaveChange(AuthorDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.SetBaseFromContext(HttpContext);
                var result = new BaseCommandResultWithData<Author>();

                if (model.Id == 0)
                {
                    var createCommand = model.ToCreateCommand();
                    result = await mediator.Send(createCommand);
                }
                else
                {
                    var updateCommand = model.ToUpdateCommand();
                    result = await mediator.Send(updateCommand);
                }

                if (result.Success)
                {
                    return Json(new
                    {
                        success = true,
                        message = AppGlobal.DefaultSuccessMessage,
                        data = result.Data

                    });
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    return Json(new
                    {
                        success = false,
                        message = result.Message
                    });
                }
            }
            else {
                return Json(new
                {
                    success = false,
                    message = ModelState.GetError()
                });
            }

        }

        public IActionResult List()
        {
            var model = new List<AuthorSummaryModel>();
            model = authorQueries.GetAll(); 
            return PartialView(model);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var command = new DeleteAuthor()
            {
                Id = Id,
                RequestId = HttpContext.Connection?.Id,
                UserName = User?.Claims?.FirstOrDefault(x => x.Type == "UserName")?.Value,
                IpAddress = HttpContext.Connection?.RemoteIpAddress?.ToString()
            };

            var result = await mediator.Send(command);

            return Json(new {
                success = result.Success,
                message = result.Message
            });
        }
    }
}
