using Orleans;

namespace OrleansTest.Orleans.Contracts.Grains;

public interface ITestGrain : IGrainWithStringKey
{
    Task<string> Hi();
}