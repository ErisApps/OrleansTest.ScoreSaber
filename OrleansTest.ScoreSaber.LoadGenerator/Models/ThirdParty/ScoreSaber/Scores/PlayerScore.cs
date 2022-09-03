using System.Text.Json.Serialization;

namespace OrleansTest.ScoreSaber.LoadGenerator.Models.ThirdParty.ScoreSaber.Scores
{
	public readonly struct PlayerScore
	{
		[JsonPropertyName("score")]
		public Score Score { get; }

		[JsonPropertyName("leaderboard")]
		public LeaderboardInfo Leaderboard { get; }

		[JsonConstructor]
		public PlayerScore(Score score, LeaderboardInfo leaderboard)
		{
			Score = score;
			Leaderboard = leaderboard;
		}
	}
}