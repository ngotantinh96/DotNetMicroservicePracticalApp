using PlatformService.Models;

namespace PlatformService.Data;

public class PlatformRepository : IPlatformRepository
{
    private readonly AppDbContext dbContext;

    public PlatformRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Add(Platform platform)
    {
        if(platform is null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        dbContext.Add(platform);
    }

    public IEnumerable<Platform> GetAlls()
    {
        return dbContext.Platforms.ToList();
    }

    public Platform GetById(int id)
    {
        return dbContext.Platforms.FirstOrDefault(x => x.Id == id);
    }

    public bool SaveChanges()
    {
        return dbContext.SaveChanges() >= 0;
    }
}
