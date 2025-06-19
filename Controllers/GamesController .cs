using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameHub.API.Data;
using GameHub.API.Models;
using Microsoft.AspNetCore.Authorization;
using GameHub.API.Dtos;
using GameHub.API.DTOs;

namespace GameHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly AppDbContext _context;

    public GamesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/games
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Game>>> GetGames()
    {
        return await _context.Games.ToListAsync();
    }

    // GET: api/games/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Game>> GetGame(int id)
    {
        var game = await _context.Games
    .Include(g => g.Reviews)
    .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
            return NotFound();

        var result = new GameDetailsDto
        {
            Id = game.Id,
            Title = game.Title,
            Description = game.Description,
            Genre = game.Genre,
            Rating = game.Rating,
            ReleaseDate = game.ReleaseDate,
            CoverUrl = game.CoverUrl,
            Reviews = game.Reviews?.Select(r => new ReviewCreateDto
            {
                Rating = r.Rating,
                Comment = r.Comment
            }).ToList() ?? new()
        };

        return Ok(result);

    }

    // POST: api/games
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Game>> CreateGame(Game game)
    {
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
    }

    // PUT: api/games/5
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGame(int id, Game game)
    {
        if (id != game.Id)
            return BadRequest();

        _context.Entry(game).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Games.Any(g => g.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: api/games/5
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null)
            return NotFound();

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
