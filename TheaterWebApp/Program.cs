using TheaterWebApp.Contexts;
using TheaterWebApp.Seeder;
using TheaterWebApp.Service;

namespace TheaterWebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<TheaterContext>();
        builder.Services.AddScoped<ITheaterService, TheaterService>();

        var app = builder.Build();
        
        using var scope = app.Services.CreateScope();
        var theaterContext = scope.ServiceProvider.GetRequiredService<TheaterContext>();
        // theaterContext.Database.EnsureDeleted();
        // theaterContext.Database.EnsureCreated();
        DataSeeder.SeedUsers(theaterContext);
        DataSeeder.SeedMovies(theaterContext);

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
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}