using PlatformService.Models;

namespace PlatformService.Data;

public interface IPlatformRepository
{
    IEnumerable<Platform> GetAlls();

    Platform GetById(int id);

    void Add(Platform platform);

    bool SaveChanges();
}