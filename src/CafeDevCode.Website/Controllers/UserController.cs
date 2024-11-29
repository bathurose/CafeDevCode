using AutoMapper;
using CafeDevCode.Common.Shared.Model;
using CafeDevCode.Database.Entities;
using CafeDevCode.Logic.Commands.Request;
using CafeDevCode.Logic.Queries.Implement;
using CafeDevCode.Logic.Queries.Interface;
using CafeDevCode.Logic.Shared.Models;
using CafeDevCode.Ultils.Extensions;
using CafeDevCode.Ultils.Global;
using CafeDevCode.Website.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.Claims;

namespace CafeDevCode.Website.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator meditor;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IUserQueries userQueries;

        public UserController(IMediator mediator, SignInManager<User> signInManager, UserManager<User> userManager, IUserQueries userQueries) 
        {
            this.meditor = mediator;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userQueries = userQueries;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region List
        [HttpGet]
        public IActionResult List()
        {
            var model = new List<UserSummaryModel>();
            model = userQueries.GetAll();
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult Detail(string userName)
        {
            var user = new UserDetailModel();
            if (userName != null)
            {
                user = userQueries.GetDetail(userName);
            }
            return PartialView(user);
        }
        #endregion 

        #region Login
        [HttpGet]
        public IActionResult AdminLogin(LoginViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AdminLoginSubmit(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = model.ToCommand();
                var result = await meditor.Send(command);

                if (result.Success)
                {
                    var user = result.Data;

                    var claims = new List<Claim>()
                    {
                        new Claim("UserName", user!.UserName),
                        new Claim("AuthorId", user!.AuthorId ?? string.Empty)
                    };

                    var claimIdentity = new ClaimsIdentity(claims);

                    var claimPrincipal = new ClaimsPrincipal(claimIdentity);

                    await signInManager.SignInAsync(user, model.RememberPassword, claimPrincipal.ToString());

                    if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(model.ReturnUrl);
                    }
                }
                else {
                    model.ErrorMessage = result.Messages;
                    return RedirectToAction("AdminLogin", model);
                }
                
            }
            else 
            {
                model.ErrorMessage = ModelState.GetError();   
                return RedirectToAction("AdminLogin", model);
            }
        }
        #endregion

        #region Create_Update
        [HttpPost]
        public async Task<IActionResult> SaveChange(UserDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.SetBaseFromContext(HttpContext);

                var commandResult = new BaseCommandResultWithData<User>();

                if (!userQueries.IsExistUserName(model.DetailUserName ?? string.Empty))
                {
                    var createCommand = model.ToCreateCommand();
                    commandResult = await meditor.Send(createCommand);
                }
                else
                {
                    var updateCommand = model.ToUpdateCommand();
                    commandResult = await meditor.Send(updateCommand);
                }

                if (commandResult.Success)
                {
                    return Json(new
                    {
                        success = true,
                        message = AppGlobal.DefaultSuccessMessage,
                        data = commandResult.Data
                    });
                }
                else
                {
                    ModelState.AddModelError("", commandResult.Messages);
                    return Json(new
                    {
                        success = false,
                        message = commandResult.Messages
                    });
                }
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = ModelState.GetError()
                });
            }
        }

        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string userName)
        {
            var commandDelete = new DeleteUser()
            {
                DeleteUserName = userName,
                RequestId = HttpContext.Connection?.Id,
                IpAddress = HttpContext.Connection?.RemoteIpAddress?.ToString(),
                UserName  = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == "UserName")?.Value
            };

            var commandResult = await meditor.Send(commandDelete);

            return Json(new
            {
                success = commandResult.Success,
                message = commandResult.Messages
            });
            
        }
        #endregion

        #region ResetPassword
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm(ResetPasswordViewModel model)
        { 
            if (ModelState.IsValid)
            {
                model.SetBaseFromContext(HttpContext);
                var commandResult = new BaseCommandResult();
                var resetPasswordCommand = model.ToResetPasswordCommand();
                commandResult = await meditor.Send(resetPasswordCommand);
                return Json(new
                {
                    success = commandResult.Success,
                    message = commandResult.Messages
                });
            }
            return Json(new
            {
                success = false,
                message = ModelState.GetError()
            });
        }
        #endregion
    }
}
