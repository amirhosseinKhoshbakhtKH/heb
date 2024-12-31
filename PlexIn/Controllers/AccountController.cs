using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlexIn.Models;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    // Constructor برای مقداردهی _context
    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Message = "Username and password cannot be empty.";
            return View();
        }

        // بررسی تکراری نبودن نام کاربر
        if (await _context.Businesses.AnyAsync(b => b.Username == username))
        {
            ViewBag.Message = "This username is already taken. Please choose another one.";
            return View();
        }

        // اضافه کردن کاربر جدید به پایگاه داده
        var newBusiness = new Business
        {
            Username = username,
            Password = BCrypt.Net.BCrypt.HashPassword(password)
        };
        _context.Businesses.Add(newBusiness);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Registration successful! Please log in.";
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Message = "Username and password cannot be empty.";
            return View();
        }

        var business = await _context.Businesses.SingleOrDefaultAsync(b => b.Username == username);

        if (business == null || !BCrypt.Net.BCrypt.Verify(password, business.Password))
        {
            ViewBag.Message = "Invalid username or password.";
            return View();
        }

        // ایجاد Claims برای کاربر
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, business.Username),
            new Claim("BusinessId", business.Id.ToString()) // ذخیره BusinessId
        };

        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

        // ورود کاربر
        await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));

        TempData["Message"] = "Login successful! Welcome to the Admin Panel.";
        return RedirectToAction("AdminPanel", "Account");
    }

    [HttpGet]
    public IActionResult Welcome(string username)
    {
        ViewBag.Username = username;
        return View();
    }

    [Authorize]
    [HttpGet]
    public IActionResult AdminPanel()
    {
        var username = User.Identity?.Name; // دریافت نام کاربری
        var businessId = User.Claims.FirstOrDefault(c => c.Type == "BusinessId")?.Value; // دریافت BusinessId

        ViewBag.Username = username;
        ViewBag.BusinessId = businessId;

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        // خروج کاربر
        await HttpContext.SignOutAsync("Cookies");

        TempData["Message"] = "You have been logged out successfully.";
        return RedirectToAction("Login");
    }
}
