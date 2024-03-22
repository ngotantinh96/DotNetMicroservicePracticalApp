using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Profiles;

public class CommandsServiceProfile : Profile
{
    public CommandsServiceProfile()
    {
        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformPublishedDto, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
        CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Commands, opt => opt.Ignore());
    }
}