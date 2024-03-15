using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataService.Http;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository platformRepository;
    private readonly IMapper mapper;
    private readonly ICommandDataClient commandDataClient;
    private readonly IMessageBusClient messageBusClient;

    public PlatformsController(IPlatformRepository platformRepository, IMapper mapper, ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
    {
        this.platformRepository = platformRepository;
        this.mapper = mapper;
        this.commandDataClient = commandDataClient;
        this.messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        Console.WriteLine("----> Getting Platforms....");
        var platforms = platformRepository.GetAlls();
        return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id) 
    {
        Console.WriteLine($"----> Getting Platform By Id {id}....");
        var platform = platformRepository.GetById(id);

        if(platform is null)
            return NotFound();

        return Ok(mapper.Map<PlatformReadDto>(platform));
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        var platform = mapper.Map<Platform>(platformCreateDto);
        platformRepository.Add(platform);
        platformRepository.SaveChanges();

        var platformReadDto = mapper.Map<PlatformReadDto>(platform);

        /// Send message synchoronously
        try
        {
            await commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not send synchoronously: {ex.Message}");
        }

        /// Send message asynchoronously
        try
        {
            var platformPublishedDto = mapper.Map<PlatformPublishedDto>(platformReadDto);
            platformPublishedDto.Event = "Platform_Published";
            messageBusClient.PublishNewPlatform(platformPublishedDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not send asynchoronously: {ex.Message}");
        }

        return CreatedAtRoute(nameof(GetPlatformById), new { Id = platform.Id}, platformReadDto);
    }
}