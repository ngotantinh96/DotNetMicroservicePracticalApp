using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controller;

[ApiController]
[Route("api/[controller]")]
public class CommandPlatformsController : ControllerBase
{
    public CommandPlatformsController()
    {
        
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("----> Connection is coming");
        return Ok();
    }
}