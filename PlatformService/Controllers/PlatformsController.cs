using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository platformRepository;
    private readonly IMapper mapper;

    public PlatformsController(IPlatformRepository platformRepository, IMapper mapper)
    {
        this.platformRepository = platformRepository;
        this.mapper = mapper;
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
    public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        var platform = mapper.Map<Platform>(platformCreateDto);
        platformRepository.Add(platform);
        platformRepository.SaveChanges();

        var platformReadDto = mapper.Map<PlatformReadDto>(platform);
        return CreatedAtRoute(nameof(GetPlatformById), new { Id = platform.Id}, platformReadDto);
    }
}