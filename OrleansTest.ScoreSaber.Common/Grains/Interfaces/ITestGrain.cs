using Orleans;

namespace OrleansTest.ScoreSaber.Common.Grains.Interfaces;

public interface ITestGrain : IGrainWithStringKey
{
    ValueTask<string> Hi();
}