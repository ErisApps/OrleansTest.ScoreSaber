using Orleans;

namespace OrleansTest.ScoreSaber.Common.Grains.Interfaces;

public interface ICountryGrain : IGrainWithStringKey
{
    ValueTask<string> GetInfo();

    ValueTask Update(DateTime lastScoreSet);
}