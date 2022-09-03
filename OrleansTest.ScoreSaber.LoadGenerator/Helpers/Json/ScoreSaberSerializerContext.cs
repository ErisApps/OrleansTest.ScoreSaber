using System.Text.Json.Serialization;
using OrleansTest.ScoreSaber.LoadGenerator.Models.ThirdParty.ScoreSaber.Websocket;

namespace OrleansTest.ScoreSaber.LoadGenerator.Helpers.Json;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(ScoreCommand))]
public partial class ScoreSaberSerializerContext : JsonSerializerContext
{
}