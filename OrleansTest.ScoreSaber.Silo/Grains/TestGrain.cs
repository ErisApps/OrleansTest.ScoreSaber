using Orleans;
using OrleansTest.ScoreSaber.Common.Grains.Interfaces;

namespace OrleansTest.ScoreSaber.Silo.Grains;

public class TestGrain : Grain, ITestGrain
{
    public ValueTask<string> Hi()
    {
        return ValueTask.FromResult<string>($"Hi from {this.GetPrimaryKeyString()}");
    }
}