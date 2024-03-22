using CommandsService.Models;

namespace CommandsService.SyncDataServices.Grpc;

public interface IPlaformDataClient
{
    IEnumerable<Platform> GetAllPlatforms();
}