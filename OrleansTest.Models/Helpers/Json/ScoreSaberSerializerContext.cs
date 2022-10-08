using System.Text.Json.Serialization;
using OrleansTest.Models.Models.ThirdParty.ScoreSaber.Websocket;

namespace OrleansTest.Models.Helpers.Json;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(ScoreCommand))]
public partial class ScoreSaberSerializerContext : JsonSerializerContext
{
}