namespace PlexIn.Controllers
{
    using BCrypt.Net;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("api/[controller]")]
    public class BusinessController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BusinessController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Business business)
        {
            if (await _context.Businesses.AnyAsync(u => u.Username == business.Username))
                return BadRequest("Username already exists");

            business.Password = HashPassword(business.Password);
            _context.Businesses.Add(business);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Business user)
        {
            var dbUser = await _context.Businesses.SingleOrDefaultAsync(u => u.Username == user.Username);
            if (dbUser == null || !VerifyPassword(user.Password, dbUser.Password))
                return Unauthorized("Invalid username or password");

            return Ok("Login successful");
        }

        private string HashPassword(string password)
        {
            var salt = BCrypt.GenerateSalt(12); // Specify the cost factor for salt generation
            var hashedPassword = BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Verify(password, hashedPassword);
        }
    }

}
