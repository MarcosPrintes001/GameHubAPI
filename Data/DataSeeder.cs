using GameHub.API.Data;
using GameHub.API.Models;
using GameHub.API.Services.Auth;

public static class DataSeeder
{
    public static void Seed(AppDbContext context, AuthService authService)
    {
        if (!context.Games.Any())
        {
            context.Games.AddRange(
                new Game { Title = "The Witcher 3", Genre = "RPG", Rating = 9.8, ReleaseDate = new DateTime(2015, 5, 19), CoverUrl = "url_witcher3" },
                new Game { Title = "Hades", Genre = "Roguelike", Rating = 9.5, ReleaseDate = new DateTime(2020, 9, 17), CoverUrl = "url_hades" }
            );
            context.SaveChanges();
        }

        if (!context.Users.Any())
        {
            CreateUser("Alice", "alice@example.com", "Senha123!", context, authService);
            CreateUser("Bob", "bob@example.com", "Senha123!", context, authService);
            context.SaveChanges();
        }
    }

    private static void CreateUser(string username, string email, string password, AppDbContext context, AuthService authService)
    {
        authService.CreatePasswordHash(password, out byte[] hash, out byte[] salt);

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        context.Users.Add(user);
    }
}
