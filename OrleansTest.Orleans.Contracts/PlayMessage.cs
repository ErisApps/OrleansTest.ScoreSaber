using Orleans.Concurrency;

namespace OrleansTest.Orleans.Contracts;

[Immutable]
public record class PlayMessage(
    long PlayerId,
    string PlayerName,
    string Country,
    DateTime Timestamp
);