using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly IMapper mapper;

    public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
    {
        this.serviceScopeFactory = serviceScopeFactory;
        this.mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch(eventType) 
        {
            case EventType.PlatformPublished:
                AddMessage(message);
                break;
            default:
                break;
        }
    }

    private void AddMessage(string platformPublishedMessage)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
        var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

        try
        {
            var platform = mapper.Map<Platform>(platformPublishedDto);
            if (!commandRepository.ExternalPlatformExists(platform.ExternalId))
            {
                commandRepository.CreatePlatform(platform);
                commandRepository.SaveChanges();
                Console.WriteLine("----> Platform Added");
            }
            else
            {
                Console.WriteLine("----> Platform Existed");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Error when adding platform to db: {ex.Message}");
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine("--> Determining Event Type");
        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        if (eventType is null)
        {
            return EventType.Undetermined;
        }

        return eventType.Event switch 
        {
            "Platform_Published" => EventType.PlatformPublished,
            _ => EventType.Undetermined
        };
    }
}

enum EventType
{
    PlatformPublished,
    Undetermined
}
