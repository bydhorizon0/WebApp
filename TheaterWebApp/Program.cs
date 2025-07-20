using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheaterWebApp.Contexts;
using TheaterWebApp.Entities;
using TheaterWebApp.Service;

namespace TheaterWebApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // builder.Services.AddHostedService<MovieDataHostedService>();

        builder.Services.AddControllersWithViews();
        builder.Services.AddAuthentication()
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Auth/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
                options.Cookie.Name = "TheaterWebApp";
                //options.Cookie.SameSite = SameSiteMode.None;
                //options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
        
        builder.Services.AddDbContext<TheaterContext>(options =>
        {
            string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            if (connectionString is null)
                throw new InvalidOperationException("The connection string already exists");

            options.UseMySQL(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
        });
        builder.Services.AddScoped<PasswordHasher<User>>();
        builder.Services.AddScoped<ITheaterService, TheaterService>();
        builder.Services.AddScoped<IUserService, UserService>();

        var app = builder.Build();

        // using var scope = app.Services.CreateScope();
        // var theaterContext = scope.ServiceProvider.GetRequiredService<TheaterContext>();
        // var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        // theaterContext.Database.EnsureDeleted();
        // theaterContext.Database.EnsureCreated();
        // var seeder = new DataSeeder(theaterContext, env);
        // await seeder.SeedMoviesAsync();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Theater}/{action=List}/{id?}");

        app.Run();
    }
}