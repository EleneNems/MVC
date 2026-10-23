using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Quiz1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<UserStore>();
builder.Services.AddSingleton<TripStore>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Account/Login";
        o.AccessDeniedPath = "/Account/CreatePassword";
        o.ExpireTimeSpan = TimeSpan.FromHours(2);
    });

builder.Services.AddAuthorization(o =>
    o.AddPolicy("FullAccess", p => p
        .RequireAuthenticatedUser()
        .RequireAssertion(ctx => !ctx.User.HasClaim("MustChangePassword", "true"))));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Account/Login");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("default", "{controller=Account}/{action=Login}/{id?}");

app.Run();
