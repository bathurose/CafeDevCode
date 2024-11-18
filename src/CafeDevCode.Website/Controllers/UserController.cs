using CafeDevCode.Database.Entities;
using CafeDevCode.Ultils.Extensions;
using CafeDevCode.Website.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CafeDevCode.Website.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator meditor;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public UserController(IMediator mediator, SignInManager<User> signInManager, UserManager<User> userManager) 
        {
            this.meditor = mediator;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

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

                    await signInManager.SignInAsync(user, model.RememberPassword);

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
                    model.ErrorMessage = result.Message;
                    return RedirectToAction("AdminLogin", model);
                }
                
            }
            else 
            {
                model.ErrorMessage = ModelState.GetError();   
                return RedirectToAction("AdminLogin", model);
            }
        }
    }
}
