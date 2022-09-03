using Orleans;
using OrleansTest.ScoreSaber.Common.Grains.Interfaces;
using OrleansTest.ScoreSaber.Common.Models;

namespace OrleansTest.ScoreSaber.Silo.Grains;

public class PlayerGrain : Grain, IPlayerGrain
{
    private readonly IClusterClient _clusterClient;

    private string? _name;
    private DateTime _lastScoreSet;
    private uint _invocationCount;

    public PlayerGrain(IClusterClient clusterClient)
    {
        _clusterClient = clusterClient;
    }

    public ValueTask<string> GetInfo()
    {
        return ValueTask.FromResult<string>($"Player {_name} set {_invocationCount} score(s). Last score was set at {_lastScoreSet} UTC.");
    }

    public ValueTask Update(PlayMessage message)
    {
        _name = message.PlayerName;
        _lastScoreSet = message.Timestamp;
        _invocationCount++;

        return _clusterClient.GetGrain<ICountryGrain>(message.Country).Update(message.Timestamp);
    }
}