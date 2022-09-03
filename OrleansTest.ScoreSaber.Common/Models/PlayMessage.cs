using Orleans.Concurrency;

namespace OrleansTest.ScoreSaber.Common.Models;

[Immutable]
public record class PlayMessage(
    long PlayerId,
    string PlayerName,
    string Country,
    DateTime Timestamp
);