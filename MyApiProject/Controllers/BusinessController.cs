using Microsoft.AspNetCore.Mvc;
using MySharedModels;

[ApiController]
[Route("api/[controller]")]
public class BusinessController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BusinessController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBusinesses()
    {
        var businesses = await _context.Businesses.ToListAsync();
        return Ok(businesses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBusinessById(int id)
    {
        var business = await _context.Businesses
            .Include(b => b.Categories)
            .Include(b => b.Features)
            .Include(b => b.Menus)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (business == null) return NotFound();

        return Ok(business);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBusiness([FromBody] Business business)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        _context.Businesses.Add(business);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBusinessById), new { id = business.Id }, business);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBusiness(int id, [FromBody] Business business)
    {
        if (id != business.Id) return BadRequest();

        _context.Entry(business).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Businesses.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBusiness(int id)
    {
        var business = await _context.Businesses.FindAsync(id);
        if (business == null) return NotFound();

        _context.Businesses.Remove(business);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
