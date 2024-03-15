using CommandsService.Models;

namespace CommandsService.Data;

public interface ICommandRepository
{
    bool SaveChanges();

    //Platforms
    IEnumerable<Platform> GetPlatforms();
    void CreatePlatform(Platform platform);
    bool PlatformExists(int platformId);
    bool ExternalPlatformExists(int externalPlatformId);
    
    //Commands
    IEnumerable<Command> GetCommandsforPlatform(int platformId);
    Command GetCommand(int platformId, int commandId);
    void CreateCommand(int platformId, Command command);
}