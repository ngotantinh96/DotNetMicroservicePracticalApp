using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataService.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;

    public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.configuration = configuration;
    }

    public async Task SendPlatformToCommand(PlatformReadDto platform)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(platform),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PostAsync(configuration["CommandsServiceEndpoint"], httpContent);

        if(response.IsSuccessStatusCode) 
        {
            Console.WriteLine("---> Send data to command service OK");
        } else 
        {
            Console.WriteLine("---> Send data to command service Not OK");
        }
    }
}