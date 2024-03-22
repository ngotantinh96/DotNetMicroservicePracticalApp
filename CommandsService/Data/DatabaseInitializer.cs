using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data;

public class DatabaseInitializer
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        var grpClient = serviceScope.ServiceProvider.GetRequiredService<IPlaformDataClient>();
        var platforms = grpClient.GetAllPlatforms();
        var commandRepository =  serviceScope.ServiceProvider.GetRequiredService<ICommandRepository>();
        SeedData(commandRepository, platforms);
    }

    private static void SeedData(ICommandRepository commandRepository, IEnumerable<Platform> platforms)
    {
        Console.WriteLine("--> Seeding new platforms...");

        foreach(var platform in platforms)
        {
            if(!commandRepository.ExternalPlatformExists(platform.ExternalId))
            {
                commandRepository.CreatePlatform(platform);
            }
        }

        commandRepository.SaveChanges();
    }
}