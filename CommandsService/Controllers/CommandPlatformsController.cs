using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controller;

[ApiController]
[Route("api/[controller]")]
public class CommandPlatformsController : ControllerBase
{
    private readonly ICommandRepository commandRepository;
    private readonly IMapper mapper;

    public CommandPlatformsController(ICommandRepository commandRepository, IMapper mapper)
    {
        this.commandRepository = commandRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        Console.WriteLine("----> Getting Platforms from CommandsService");
        var platformItems = commandRepository.GetPlatforms();
        return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("----> Connection is coming");
        return Ok();
    }
}