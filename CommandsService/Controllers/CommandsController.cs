using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controller;

[ApiController]
[Route("api/[controller]/{platformId}")]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepository commandRepository;
    private readonly IMapper mapper;

    public CommandsController(ICommandRepository commandRepository, IMapper mapper)
    {
        this.commandRepository = commandRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
        Console.WriteLine($"----> Getting GetCommandsForPlatform: {platformId}");
        
        if(!commandRepository.PlatformExists(platformId)) 
        {
            return NotFound();
        }

        var commands = commandRepository.GetCommandsforPlatform(platformId);
        return Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine($"----> Getting GetCommandForPlatform: {platformId} / {commandId}");
        
        if(!commandRepository.PlatformExists(platformId)) 
        {
            return NotFound();
        }

        var command = commandRepository.GetCommand(platformId, commandId);

        if(command is null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<CommandReadDto>(command));
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
    {
         Console.WriteLine($"----> Getting CreateCommandForPlatform: {platformId}");
        
        if(!commandRepository.PlatformExists(platformId)) 
        {
            return NotFound();
        }

        var command = mapper.Map<Command>(commandDto);
        commandRepository.CreateCommand(platformId, command);
        commandRepository.SaveChanges();

        var commandReadDto = mapper.Map<CommandReadDto>(command);

        return CreatedAtRoute(nameof(GetCommandForPlatform),
            new { platformId, commandId = commandReadDto.Id }, commandReadDto);
    }
}