using TheaterWebApp.Entities;
using TheaterWebApp.Models;

namespace TheaterWebApp.Service;

public interface IUserService
{
    Task RegisterAsync(RegisterRequest request);
    Task<User?> LoginAsync(LoginRequest request);
    Task<User> GetUserByEmailAsync(string email);
}