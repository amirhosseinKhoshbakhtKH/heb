[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CustomerController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await _context.Customers
            .Include(c => c.StarTransactions)
            .Include(c => c.DiscountCodes)
            .ToListAsync();

        return Ok(customers);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllCustomers), new { id = customer.Id }, customer);
    }

    [HttpPost("{customerId}/stars")]
    public async Task<IActionResult> AddStars(int customerId, [FromBody] StarTransaction transaction)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null) return NotFound();

        transaction.CustomerId = customerId;
        _context.StarTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        return Ok(transaction);
    }
}
