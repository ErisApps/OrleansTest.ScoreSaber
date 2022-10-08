using Orleans;
using Orleans.Runtime;
using OrleansTest.Orleans.Contracts.Grains;

namespace OrleansTest.Orleans.Grains.Grains;

public class CountryState
{
	public DateTime LastScoreSet { get; set; }
	public uint InvocationCount { get; set; }
}

public class CountryGrain : Grain, ICountryGrain
{
	private readonly IPersistentState<CountryState> _state;

	public CountryGrain([PersistentState(nameof(CountryState), "countryStore")] IPersistentState<CountryState> state)
	{
		_state = state;
	}

	public ValueTask<string> GetInfo()
	{
		if (!_state.RecordExists)
		{
			return ValueTask.FromResult<string>($"No songs were played in this {this.GetPrimaryKeyString()}.");
		}

		return ValueTask.FromResult<string>($"{_state.State.InvocationCount} song(s) were played by players in {this.GetPrimaryKeyString()}. Last score was set at {_state.State.LastScoreSet} UTC.");
	}

	public Task Update(DateTime lastScoreSet)
	{
		_state.State.LastScoreSet = lastScoreSet;
		_state.State.InvocationCount++;

		return _state.WriteStateAsync();
	}
}