using CommandsService.Models;

namespace CommandsService.Data;

public class CommandRepository : ICommandRepository
{
    private readonly AppDbContext dbContext;

    public CommandRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void CreateCommand(int platformId, Command command)
    {
        if(command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        command.PlatformId = platformId;
        dbContext.Commands.Add(command);
    }

    public void CreatePlatform(Platform platform)
    {
        if(platform is null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        dbContext.Platforms.Add(platform);
    }

    public Command GetCommand(int platformId, int commandId)
    {
        return dbContext.Commands.FirstOrDefault(x => x.PlatformId == platformId && x.Id == commandId);
    }

    public IEnumerable<Command> GetCommandsforPlatform(int platformId)
    {
        return dbContext.Commands
            .Where(x => x.PlatformId == platformId)
            .OrderBy(x => x.PlatformId);
    }

    public IEnumerable<Platform> GetPlatforms()
    {
        return dbContext.Platforms.ToList();
    }

    public bool PlatformExists(int platformId)
    {
       return dbContext.Platforms.Any(x => x.Id == platformId);
    }

    public bool SaveChanges()
    {
        return dbContext.SaveChanges() >= 0;
    }
}
