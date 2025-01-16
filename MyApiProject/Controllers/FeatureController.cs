[ApiController]
[Route("api/[controller]")]
public class FeatureController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FeatureController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFeatures()
    {
        var features = await _context.Features
            .Include(f => f.Options)
            .ToListAsync();

        return Ok(features);
    }

    [HttpPost]
    public async Task<IActionResult> CreateFeature([FromBody] Feature feature)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        _context.Features.Add(feature);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllFeatures), new { id = feature.Id }, feature);
    }

    [HttpPost("{featureId}/options")]
    public async Task<IActionResult> AddFeatureOption(int featureId, [FromBody] FeatureOption option)
    {
        var feature = await _context.Features.FindAsync(featureId);
        if (feature == null) return NotFound();

        option.FeatureId = featureId;
        _context.FeatureOptions.Add(option);
        await _context.SaveChangesAsync();

        return Ok(option);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFeature(int id)
    {
        var feature = await _context.Features.FindAsync(id);
        if (feature == null) return NotFound();

        _context.Features.Remove(feature);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
