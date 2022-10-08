using Orleans;

namespace OrleansTest.Orleans.Contracts.Grains;

public interface IPlayerGrain : IGrainWithIntegerKey
{
    ValueTask<string> GetInfo();
    Task Update(PlayMessage message);
}