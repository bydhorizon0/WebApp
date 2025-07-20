using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheaterWebApp.Contexts;
using TheaterWebApp.Entities;
using TheaterWebApp.Models;

namespace TheaterWebApp.Service;

public class UserService : IUserService
{
    private readonly TheaterContext _context;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(TheaterContext context, PasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }
    
    public async Task RegisterAsync(RegisterRequest request)
    {
        // 중복 검사
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            throw new InvalidOperationException("User email already exists");
        if (await _context.Users.AnyAsync(u => u.Nickname == request.Nickname))
            throw new InvalidOperationException("User nickname already exists");

        var user = new User
        {
            Email = request.Email,
            Nickname = request.Nickname,
        };
        
        user.Password = _passwordHasher.HashPassword(user, request.Password);
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> LoginAsync(LoginRequest request)
    {
        var user = await GetUserByEmailAsync(request.Email);

        if (_passwordHasher.VerifyHashedPassword(user, user.Password, request.Password) == PasswordVerificationResult.Success)
        {
            return user;
        }
        return null;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email) ?? throw new InvalidOperationException($"{email} User not found");
    }
}