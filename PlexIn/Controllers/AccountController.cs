using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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


    // افزودن به متد Register (HttpPost)
    [HttpPost]
    public async Task<IActionResult> Register(string username, string password)
    {
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

        ViewBag.Message = "Registration successful!";
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var business = await _context.Businesses.SingleOrDefaultAsync(b => b.Username == username);

        if (business == null || !BCrypt.Net.BCrypt.Verify(password, business.Password))
        {
            ViewBag.Message = "Invalid username or password.";
            return View();
        }

        // تولید توکن JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("mymyselfVeryStrongSecretKeyWithMinimum32Characters!");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, business.Username),
            new Claim("Role", "Admin") // نقش ادمین
        }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = "yourapp.com",
            Audience = "yourapp.com",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        // ذخیره توکن در کوکی یا فرانت‌اند
        HttpContext.Response.Cookies.Append("AuthToken", tokenString, new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // در محیط تولید این مقدار را به true تغییر بده
            SameSite = SameSiteMode.Strict
        });

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
        Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Expires"] = "0";

        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var username = User.Identity?.Name; // نام کاربر از توکن JWT
        ViewBag.Username = username;
        return View();
    }

    [HttpGet]
    public IActionResult Logout()
    {
        // حذف توکن از کوکی
        HttpContext.Response.Cookies.Delete("AuthToken");
        // هدایت به صفحه لاگین
        return RedirectToAction("Login");
    }

}
