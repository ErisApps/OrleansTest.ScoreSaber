using Microsoft.AspNetCore.Mvc;
using Orleans;
using OrleansTest.Orleans.Contracts.Grains;

namespace OrleansTest.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CountryController : ControllerBase
{
    private readonly IClusterClient _clusterClient;

    public CountryController(IClusterClient clusterClient)
    {
        _clusterClient = clusterClient;
    }

    [HttpGet("/country/{countryCode}")]
    public async Task<IActionResult> GetCountryInfo(string countryCode)
    {
        var countryGrain = _clusterClient.GetGrain<ICountryGrain>(countryCode);
        var info = await countryGrain.GetInfo().ConfigureAwait(false);

        return Ok(info);
    }
}