using Orleans;
using Orleans.Runtime;
using OrleansTest.Orleans.Contracts;
using OrleansTest.Orleans.Contracts.Grains;

namespace OrleansTest.Orleans.Grains.Grains;

public class PlayerState
{
	public string? Name { get; set; }
	public DateTime LastScoreSet { get; set; }
	public uint InvocationCount { get; set; }
}

public class PlayerGrain : Grain, IPlayerGrain
{
	private readonly IClusterClient _clusterClient;
	private readonly IPersistentState<PlayerState> _state;

	public PlayerGrain(IClusterClient clusterClient, [PersistentState(nameof(PlayerState), "playerStore")] IPersistentState<PlayerState> state)
	{
		_clusterClient = clusterClient;
		_state = state;
	}

	public ValueTask<string> GetInfo()
	{
		if (!_state.RecordExists)
		{
			return ValueTask.FromResult<string>($"Player with id {this.GetPrimaryKeyString()} hasn't played any songs so far.");
		}

		return ValueTask.FromResult<string>($"Player {_state.State.Name} set {_state.State.InvocationCount} score(s). Last score was set at {_state.State.LastScoreSet} UTC.");
	}

	public async Task Update(PlayMessage message)
	{
		_state.State.Name = message.PlayerName;
		_state.State.LastScoreSet = message.Timestamp;
		_state.State.InvocationCount++;

		await _state.WriteStateAsync();

		await _clusterClient.GetGrain<ICountryGrain>(message.Country).Update(message.Timestamp);
	}
}