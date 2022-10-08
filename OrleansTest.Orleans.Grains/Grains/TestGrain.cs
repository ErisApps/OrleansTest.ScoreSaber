using Orleans;
using OrleansTest.Orleans.Contracts.Grains;

namespace OrleansTest.Orleans.Grains.Grains;

public class TestGrain : Grain, ITestGrain
{
    public async Task<string> Hi()
    {
        return await Task.FromResult<string>($"Hi from {this.GetPrimaryKeyString()}");
    }
}