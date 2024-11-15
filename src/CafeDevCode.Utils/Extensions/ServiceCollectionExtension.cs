using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Ultils.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSqlServerDatabase<TContext>(this IServiceCollection services,
            string connectionString)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
            {
                options.UseSqlServer(connectionString, c =>
                {
                    c.CommandTimeout(1000);
                    c.EnableRetryOnFailure(5);
                });
            });

            return services;
        }

        public static IServiceCollection AddExternalAuth(this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddAuthentication()
                .AddFacebook(fboptions =>
                {
                    fboptions.AppId = configuration["Authentication:Facebook:AppId"];
                    fboptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                    fboptions.AccessDeniedPath = configuration["Authentication:Facebook:AccessDeniedPath"];

                })
                .AddGoogle(ggoptions =>
                {
                    ggoptions.ClientId =  configuration["Authentication:Google:ClientId"];
                    ggoptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                });
            return services;
        }

        public static IServiceCollection AddCookiesAuthenticate(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(double.Parse(configuration.GetSection("AuthCookies: Timeout").Value));
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Forbiden";
                    options.LoginPath = configuration.GetSection("AuthCookies : LoginPath").Value;
                });
            return services;
        }

        public static IServiceCollection AddIdentityConfig<TUser, TRole, TDbContext>(this IServiceCollection services)
            where TUser : IdentityUser
            where TRole : IdentityRole
            where TDbContext : DbContext
        {
            services.AddIdentity<TUser, TRole>()
                .AddEntityFrameworkStores<TDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

            return services;
        }

    }
}
