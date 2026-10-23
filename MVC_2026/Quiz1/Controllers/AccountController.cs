using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz1.Models;
using Quiz1.Services;

namespace Quiz1.Controllers;

public class AccountController : Controller
{
    private readonly UserStore _users;
    public AccountController(UserStore users) => _users = users;

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(MustChange() ? nameof(CreatePassword) : "Index", MustChange() ? "Account" : "Trips");
        return View(new LoginViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = _users.FindByEmail(model.Email);
        if (user is null || !_users.Verify(user, model.Password))
        {
            ModelState.AddModelError("", "Email or password is incorrect.");
            return View(model);
        }

        await SignInAsync(user);

        return user.MustChangePassword
            ? RedirectToAction(nameof(CreatePassword))
            : RedirectToAction("Index", "Trips");
    }

    [Authorize, HttpGet]
    public IActionResult CreatePassword()
    {
        if (!MustChange()) return RedirectToAction("Index", "Trips");
        return View(new CreatePasswordViewModel());
    }

    [Authorize, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePassword(CreatePasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = _users.FindById(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
        if (user is null) return await Logout();

        if (_users.Verify(user, model.Password))
        {
            ModelState.AddModelError(nameof(model.Password), "New password must differ from the current one.");
            return View(model);
        }

        _users.SetPassword(user, model.Password, mustChange: false);
        await SignInAsync(user); 
        return RedirectToAction("Index", "Trips");
    }

    [HttpGet]
    public IActionResult ForgotPassword() => View(new ForgotPasswordViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = _users.FindByEmail(model.Email);
        if (user is not null)
        {
            var temp = "Reset" + RandomNumberGenerator.GetInt32(100000, 999999);
            _users.SetPassword(user, temp, mustChange: true);
            ViewBag.TempPassword = temp;
        }
        ViewBag.Sent = true;
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    private bool MustChange() => User.HasClaim("MustChangePassword", "true");

    private Task SignInAsync(User u)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, u.Id.ToString()),
            new(ClaimTypes.Email, u.Email),
            new("MustChangePassword", u.MustChangePassword ? "true" : "false")
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
        return HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}
