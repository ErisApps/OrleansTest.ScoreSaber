using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;
using IWebsocketClientLite.PCL;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Orleans;
using OrleansTest.Models.Helpers.Json;
using OrleansTest.Orleans.Contracts;
using OrleansTest.Orleans.Contracts.Grains;
using WebsocketClientLite.PCL;
using WebsocketClientLite.PCL.CustomException;

namespace OrleansTest.ScoreSaber.LoadGenerator;

public class WebSocketWorker : BackgroundService
{
	private readonly IClusterClient _clusterClient;

	public WebSocketWorker(IClusterClient clusterClient)
	{
		_clusterClient = clusterClient;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web) { PropertyNameCaseInsensitive = false }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
		var scoreSaberSerializerContext = new ScoreSaberSerializerContext(jsonSerializerOptions);

		var websocketClient = new MessageWebsocketRx { ExcludeZeroApplicationDataInPong = true };
		var websocketConnectObservable = websocketClient
			.WebsocketConnectWithStatusObservable(new Uri("ws://scoresaber.com/ws"), handshakeTimeout: TimeSpan.FromSeconds(15))
			// .Do(static dataframe => Console.WriteLine(dataframe?.Message))
			.ObserveOn(System.Reactive.Concurrency.ThreadPoolScheduler.Instance)
			.Catch<(IDataframe? dataframe, ConnectionStatus state), WebsocketClientLiteTcpConnectException>(
				_ => Observable.Return<(IDataframe? dataframe, ConnectionStatus state)>((null, ConnectionStatus.ConnectionFailed)))
			.Catch<(IDataframe? dataframe, ConnectionStatus state), WebsocketClientLiteException>(_ =>
			{
				Console.WriteLine("An websocket error occurred. Returning Continuation status as reference marker");
				return Observable.Return<(IDataframe? dataframe, ConnectionStatus state)>((null, ConnectionStatus.Continuation));
			});

		var websocketConnectionSubject = new Subject<(IDataframe? dataframe, ConnectionStatus state)>();

		var connectionStatusObservable = websocketConnectionSubject
			.Do(static tuple => Console.WriteLine($"Current state: {tuple.state}"))
			.Subscribe();

		var messageReceivedObservable = websocketConnectionSubject
			.Where(static tuple =>
				tuple.state == ConnectionStatus.DataframeReceived && tuple.dataframe?.Message != null && tuple.dataframe.Message.StartsWith('{')) // Only dataframe messages that are json
			.Select(tuple => JsonSerializer.Deserialize(tuple.dataframe!.Message!, scoreSaberSerializerContext.ScoreCommand)!)
			.Select(static scoreCommand =>
			{
				var playerInfo = scoreCommand.CommandData.Score.LeaderboardPlayer;
				return new PlayMessage(playerInfo.Id, playerInfo.Name, playerInfo.Country, scoreCommand.CommandData.Score.TimeSet.ToDateTimeUtc());
			})
			.Where(static playMessage => !string.IsNullOrWhiteSpace(playMessage.Country)) // Silently drop players with unknown country
			.Do(static playMessage => Console.WriteLine(playMessage))
			.Select(message => Observable.FromAsync(async () =>
			{
				var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(message.PlayerId);
				await playerGrain.Update(message);
			}))
			.Concat()
			.Subscribe();

		websocketConnectObservable.Subscribe(websocketConnectionSubject);

		await Task.Delay(-1, CancellationToken.None);
	}
}