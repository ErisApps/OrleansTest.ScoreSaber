using System.Text.Json.Serialization;
using OrleansTest.Models.Models.ThirdParty.ScoreSaber.Scores;

namespace OrleansTest.Models.Models.ThirdParty.ScoreSaber.Websocket;

public class ScoreCommand : Command<PlayerScore>
{
    [JsonConstructor]
    public ScoreCommand(string commandName, PlayerScore commandData) : base(commandName, commandData)
    {
    }
}