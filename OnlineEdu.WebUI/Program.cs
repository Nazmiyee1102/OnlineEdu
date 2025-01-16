using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineEdu.Entity.Entities;
using OnlineEdu.WebUI.Validators;
using OnlineEdu.WebUI.Areas.Admin.Controllers;
using System.Reflection;
using OnlineEdu.DataAccess.Context;
using OnlineEdu.WebUI.Services.UserServices;
using OnlineEdu.WebUI.Services.RoleServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OnlineEdu.WebUI.Services.TokenServices;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient("EduClient", cfg =>
{
    var tokenService = builder.Services.BuildServiceProvider().GetRequiredService<ITokenService>();
    var token = tokenService.GetUserToken;
    cfg.BaseAddress = new Uri("https://localhost:7207/api/");
    if (token != null)
    {
        cfg.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetUserToken);
    }
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddCookie(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    opt.LoginPath = "/Login/SignIn";
    opt.LogoutPath = "/Login/Logout";
    opt.AccessDeniedPath = "/ErrorPage/AccessDenied";
    opt.Cookie.SameSite = SameSiteMode.Strict;
    opt.Cookie.HttpOnly = true;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    opt.Cookie.Name = "OnlineEduJwt";
});




builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseStatusCodePagesWithReExecute("/ErrorPage/_404NotFound");
app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
