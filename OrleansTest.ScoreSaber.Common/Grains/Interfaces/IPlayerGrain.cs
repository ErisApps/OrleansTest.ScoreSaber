using Orleans;
using OrleansTest.ScoreSaber.Common.Models;

namespace OrleansTest.ScoreSaber.Common.Grains.Interfaces;

public interface IPlayerGrain : IGrainWithIntegerKey
{
    ValueTask<string> GetInfo();
    ValueTask Update(PlayMessage message);
}