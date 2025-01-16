[ApiController]
[Route("api/[controller]")]
public class DiscountCodeController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DiscountCodeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDiscountCodes()
    {
        var codes = await _context.DiscountCodes.ToListAsync();
        return Ok(codes);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDiscountCode([FromBody] DiscountCode discountCode)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        _context.DiscountCodes.Add(discountCode);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllDiscountCodes), new { id = discountCode.Id }, discountCode);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiscountCode(int id)
    {
        var discountCode = await _context.DiscountCodes.FindAsync(id);
        if (discountCode == null) return NotFound();

        _context.DiscountCodes.Remove(discountCode);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
