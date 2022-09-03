using Microsoft.AspNetCore.Mvc;
using Orleans;
using OrleansTest.ScoreSaber.Common.Grains.Interfaces;

namespace OrleansTest.ScoreSaber.Silo.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IClusterClient _clusterClient;

    public PlayerController(IClusterClient clusterClient)
    {
        _clusterClient = clusterClient;
    }
    [HttpGet("/player/{playerId}")]
    public async Task<IActionResult> GetPlayerInfo(long playerId)
    {
        var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(playerId);
        var info = await playerGrain.GetInfo().ConfigureAwait(false);

        return Ok(info);
    }
}