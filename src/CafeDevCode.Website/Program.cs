using CafeDevCode.Database;
using CafeDevCode.Database.Entities;
using CafeDevCode.Logic.Commands.Request;
using CafeDevCode.Logic.MappingProfile;
using CafeDevCode.Ultils.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CafeDevCode.Logic;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = 
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddHttpContextAccessor();
builder.Services.AddCookiesAuthenticate(builder.Configuration);
builder.Services.AddSqlServerDatabase<AppDatabase>(builder.Configuration.GetConnectionString("Database"));
builder.Services.AddIdentityConfig<User, IdentityRole, AppDatabase>();
//fix
//builder.Services.AddMediatR(typeof(Login).Assembly);
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(Login).Assembly));
//fix
builder.Services.AddAutoMapper(typeof(AuthorMappingProfile).Assembly);
builder.Services.AddQueries();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var database = scope.ServiceProvider.GetRequiredService<AppDatabase>();
    await database.Database.MigrateAsync();
    await AppSeeder.InitializeAsync(database, userManager, roleManager);
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();