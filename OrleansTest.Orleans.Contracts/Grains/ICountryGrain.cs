using Orleans;

namespace OrleansTest.Orleans.Contracts.Grains;

public interface ICountryGrain : IGrainWithStringKey
{
    ValueTask<string> GetInfo();

    Task Update(DateTime lastScoreSet);
}