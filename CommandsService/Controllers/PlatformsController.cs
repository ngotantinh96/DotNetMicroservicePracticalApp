using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controller;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    public PlatformsController()
    {
        
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("----> Connection is coming");
        return Ok();
    }
}