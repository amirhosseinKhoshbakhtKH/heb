[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CommentController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllComments()
    {
        var comments = await _context.Comments
            .Include(c => c.Replies)
            .ToListAsync();

        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] Comment comment)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllComments), new { id = comment.Id }, comment);
    }

    [HttpPost("{commentId}/reply")]
    public async Task<IActionResult> ReplyToComment(int commentId, [FromBody] CommentReply reply)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null) return NotFound();

        reply.CommentId = commentId;
        _context.CommentReplies.Add(reply);
        await _context.SaveChangesAsync();

        return Ok(reply);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null) return NotFound();

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
