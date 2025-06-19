using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameHub.API.Data;
using GameHub.API.Models;
using Microsoft.AspNetCore.Authorization;
using GameHub.API.Dtos;

namespace GameHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReviewsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/reviews/game/5
    [HttpGet("game/{gameId}")]
    public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByGame(int gameId)
    {
        var reviews = await _context.Reviews
            .Where(r => r.GameId == gameId)
            .ToListAsync();

        return Ok(reviews);
    }

    // POST: api/reviews
    [Authorize]
    [HttpPost]
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Review>> CreateReview(ReviewCreateDto dto)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "nameid");
        if (userIdClaim == null)
            return Unauthorized();

        var review = new Review
        {
            UserId = int.Parse(userIdClaim.Value),
            GameId = dto.GameId,
            Rating = dto.Rating,
            Comment = dto.Comment
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
    }


    // GET: api/reviews/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Review>> GetReview(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
            return NotFound();

        return review;
    }

    // DELETE: api/reviews/5
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
            return NotFound();

        // Opcional: só permitir apagar quem criou a review (verificar UserId)
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "nameid");
        if (userIdClaim == null || review.UserId != int.Parse(userIdClaim.Value))
            return Forbid();

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
