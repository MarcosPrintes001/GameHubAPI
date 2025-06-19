using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameHub.API.Data;
using GameHub.API.Models;
using Microsoft.AspNetCore.Authorization;
using GameHub.API.Dtos;

namespace GameHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly AppDbContext _context;

    public FavoritesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/favorites/user/5
    [Authorize]
    [HttpGet("user")]
    public async Task<ActionResult<IEnumerable<Favorite>>> GetFavoritesByUser()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "nameid");
        if (userIdClaim == null)
            return Unauthorized();

        int userId = int.Parse(userIdClaim.Value);

        var favorites = await _context.Favorites
            .Where(f => f.UserId == userId)
            .Include(f => f.Game)
            .ToListAsync();

        return Ok(favorites);
    }

    // POST: api/favorites
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> AddFavorite(FavoriteCreateDto dto)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "nameid");
        if (userIdClaim == null)
            return Unauthorized();

        int userId = int.Parse(userIdClaim.Value);

        var exists = await _context.Favorites.AnyAsync(f => f.UserId == userId && f.GameId == dto.GameId);
        if (exists)
            return Conflict("Jogo já está favoritado.");

        var favorite = new Favorite
        {
            UserId = userId,
            GameId = dto.GameId
        };

        _context.Favorites.Add(favorite);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFavoritesByUser), null);
    }


    // DELETE: api/favorites/game/5
    [Authorize]
    [HttpDelete("game/{gameId}")]
    public async Task<IActionResult> RemoveFavorite(int gameId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "nameid");
        if (userIdClaim == null)
            return Unauthorized();

        int userId = int.Parse(userIdClaim.Value);

        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.GameId == gameId);

        if (favorite == null)
            return NotFound();

        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
