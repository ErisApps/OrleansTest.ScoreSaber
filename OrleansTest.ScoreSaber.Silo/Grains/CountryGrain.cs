using Orleans;
using OrleansTest.ScoreSaber.Common.Grains.Interfaces;

namespace OrleansTest.ScoreSaber.Silo.Grains;

public class CountryGrain : Grain, ICountryGrain
{
	private DateTime _lastScoreSet;
	private uint _invocationCount;

	public ValueTask<string> GetInfo()
	{
		return ValueTask.FromResult<string>($"{_invocationCount} song(s) were played by players in {this.GetPrimaryKeyString()}. Last score was set at {_lastScoreSet} UTC.");
	}

	public ValueTask Update(DateTime lastScoreSet)
	{
		_lastScoreSet = lastScoreSet;
		_invocationCount++;

		return ValueTask.CompletedTask;
	}
}