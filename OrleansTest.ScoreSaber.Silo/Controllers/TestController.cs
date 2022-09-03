using Microsoft.AspNetCore.Mvc;
using Orleans;
using OrleansTest.ScoreSaber.Common.Grains.Interfaces;

namespace OrleansTest.ScoreSaber.Silo.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IClusterClient _clusterClient;

    public TestController(IClusterClient clusterClient)
    {
        _clusterClient = clusterClient;
    }

    [HttpGet("/test/{testCode}")]
    public async Task<IActionResult> GetCountryInfo(string testCode)
    {
        var testGrain = _clusterClient.GetGrain<ITestGrain>(testCode);
        var info = await testGrain.Hi().ConfigureAwait(false);
        
        return Ok(info);
    }
}