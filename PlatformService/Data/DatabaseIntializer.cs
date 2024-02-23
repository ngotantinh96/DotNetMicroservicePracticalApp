using PlatformService.Models;

namespace PlatformService.Data;

public class DatabaseInitializer
{
    public static void Seed(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        SeedData(scope.ServiceProvider.GetRequiredService<AppDbContext>());
    }

    private static void SeedData(AppDbContext dbContext)
    {
        if(!dbContext.Platforms.Any())
        {
            Console.WriteLine("--> Seeding data");

            dbContext.Platforms. AddRange(
                new Platform() {Name="Dot Net", Publisher="Microsoft", Cost="Free"},
                new Platform() {Name="SQL Server Express", Publisher="Microsoft", Cost="Free"},
                new Platform() {Name="Kubernetes", Publisher="Cloud Native Computing Foundation", Cost="Free"}
            );

            dbContext.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}