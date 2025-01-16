[ApiController]
[Route("api/[controller]")]
public class CampaignController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CampaignController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCampaigns()
    {
        var campaigns = await _context.Campaigns.Include(c => c.Business).ToListAsync();
        return Ok(campaigns);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCampaign([FromBody] Campaign campaign)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        _context.Campaigns.Add(campaign);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllCampaigns), new { id = campaign.Id }, campaign);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCampaign(int id)
    {
        var campaign = await _context.Campaigns.FindAsync(id);
        if (campaign == null) return NotFound();

        _context.Campaigns.Remove(campaign);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
