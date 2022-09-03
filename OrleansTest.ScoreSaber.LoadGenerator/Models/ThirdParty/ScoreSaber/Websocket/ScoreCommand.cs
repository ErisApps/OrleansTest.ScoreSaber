using System.Text.Json.Serialization;
using OrleansTest.ScoreSaber.LoadGenerator.Models.ThirdParty.ScoreSaber.Scores;

namespace OrleansTest.ScoreSaber.LoadGenerator.Models.ThirdParty.ScoreSaber.Websocket;

public class ScoreCommand : Command<PlayerScore>
{
    [JsonConstructor]
    public ScoreCommand(string commandName, PlayerScore commandData) : base(commandName, commandData)
    {
    }
}