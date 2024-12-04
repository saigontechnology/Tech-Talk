using Microsoft.AspNetCore.Mvc;
using Sts.Sample.Grpc.ClientIntegration.Services;

namespace Sts.Sample.Grpc.ClientIntegration.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController(TestService testService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> SayHello(string? name)
    {
        var message = await testService.SayHelloAsync(name);
        return Ok(message);
    }
    
    // [HttpGet("return-error")]
    // public async Task<IActionResult> ReturnError(string? name)
    // {
    //     var message = await testService.ReturnError(name);
    //     return Ok(message);
    // }
    //
    // [HttpGet("server-stream")]
    // public async Task<IActionResult> ServerStream(string? name)
    // {
    //     await testService.ServerStream(name);
    //     return Ok();
    // }
    //
    [HttpGet("client-stream")]
    public async Task<IActionResult> ClientStream(string? name)
    {
        await testService.ClientStream(name);
        return Ok();
    }
    
    [HttpGet("code-first")]
    public async Task<IActionResult> TestCodeFirst(string? name)
    {
        var message = await testService.TestCodeFirst(name);
        return Ok(message);
    }
}