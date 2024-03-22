using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;

namespace CommandsService.SyncDataServices.Grpc;

public class PlaformDataClient : IPlaformDataClient
{
    private readonly IConfiguration configuration;
    private readonly IMapper mapper;

    public PlaformDataClient(IConfiguration configuration, IMapper mapper)
    {
        this.configuration = configuration;
        this.mapper = mapper;
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        Console.WriteLine($"--> Calling Grpc Platform Server: {configuration["GrpcPlatform"]}");
        var channel = GrpcChannel.ForAddress(configuration["GrpcPlatform"]);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        
        try 
        {
            var response = client.GetAllPlatforms(new GetAllRequest());
            return mapper.Map<IEnumerable<Platform>>(response.Platform);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"--> Error when GetAllPlatforms: {ex.Message}");
            return null;
        }
    }
}