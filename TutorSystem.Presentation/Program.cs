using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using TutorSystem.Repository;
using TutorSystem.Repository.Interfaces;
using TutorSystem.Repository.Repositories;
using TutorSystem.Service.Interfaces;
using TutorSystem.Service.Services;
using System.Security.Claims;
using TutorSystem.Repository.Entities;

var builder = WebApplication.CreateBuilder(args);

// Đọc chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Đăng ký DbContext (SQL Server)
builder.Services.AddDbContext<TutorSystemContext>(options =>
    options.UseSqlServer(connectionString));

// Đăng ký Repository & Service
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITutorRepository, TutorRepository>();
builder.Services.AddScoped<ITutorService, TutorService>();

// Đăng ký HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Cấu hình Session Middleware
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Cấu hình Authentication bằng Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddAuthorization();
builder.Services.AddRazorPages();

var app = builder.Build();

// Cấu hình HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Middleware kiểm tra trạng thái xác minh của Tutor
//app.Use(async (context, next) =>
//{
//    if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
//    {
//        var userRole = context.User.FindFirstValue(ClaimTypes.Role);
//        if (userRole == "Tutor")
//        {
//            var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
//            if (Guid.TryParse(userIdClaim, out var userId))
//            {
//                var tutorService = context.RequestServices.GetRequiredService<ITutorService>();
//                if (!await tutorService.IsTutorApprovedAsync(userId) &&
//                    !context.Request.Path.StartsWithSegments("/Account/TutorVerification"))
//                {
//                    context.Response.Redirect("/Account/TutorVerification");
//                    return;
//                }
//            }
//        }
//    }
//    await next();
//});
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapRazorPages();
app.Run();
