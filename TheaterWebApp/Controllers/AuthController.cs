using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TheaterWebApp.Models;
using TheaterWebApp.Service;

namespace TheaterWebApp.Controllers;

[Route("[controller]/[action]")]
public class AuthController : Controller
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        if (request.Password != request.ConfirmPassword)
        {
            return View(request);
        }
        
        await _userService.RegisterAsync(request);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var loginUser = await _userService.LoginAsync(request);

        if (loginUser == null)
        {
            ModelState.AddModelError(string.Empty, "이메일 또는 비밀번호가 올바르지 않습니다.");
            return View(request);   
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, loginUser.Email),
            new Claim(ClaimTypes.Name, loginUser.Nickname),
        };
        
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(
            scheme: CookieAuthenticationDefaults.AuthenticationScheme,
            principal: principal,
            properties: new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3),
            });
        
        return RedirectToAction(nameof(TheaterController.List), "Theater");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }
}