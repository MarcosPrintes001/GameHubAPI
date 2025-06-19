using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameHub.API.Data;
using GameHub.API.DTOs;
using GameHub.API.Models;
using GameHub.API.Services.Auth;
using Microsoft.AspNetCore.Authorization;

namespace GameHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly AuthService _authService;

    public UserController(AppDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Email já está em uso.");

        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            return BadRequest("Username já está em uso.");

        _authService.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = _authService.GenerateToken(user);

        var response = new UserResponseDto
        {
            Username = user.Username,
            Email = user.Email,
            Token = token
        };

        return Created("", response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            return Unauthorized("Usuário não encontrado.");

        bool validPassword = _authService.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt);

        if (!validPassword)
            return Unauthorized("Senha inválida.");

        var token = _authService.GenerateToken(user);

        var response = new UserResponseDto
        {
            Username = user.Username,
            Email = user.Email,
            Token = token
        };

        return Ok(response);
    }

    // GET: api/user
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
    {
        var users = await _context.Users
            .Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Token = "" // não expor token aqui
            })
            .ToListAsync();

        return Ok(users);
    }

    // DELETE: api/user/{id}
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound("Usuário não encontrado.");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
