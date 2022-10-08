using System.Text.Json.Serialization;

namespace OrleansTest.Models.Models.ThirdParty.ScoreSaber.Shared
{
	public class PlayerInfoBase
	{
		[JsonPropertyName("id")]
		public long Id { get; }

		[JsonPropertyName("name")]
		public string Name { get; }

		[JsonPropertyName("profilePicture")]
		public string ProfilePicture { get; }

		[JsonPropertyName("country")]
		public string Country { get; }

		[JsonConstructor]
		public PlayerInfoBase(long id, string name, string profilePicture, string country)
		{
			Id = id;
			Name = name;
			ProfilePicture = profilePicture;
			Country = country;
		}
	}
}