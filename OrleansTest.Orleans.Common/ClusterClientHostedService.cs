using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace OrleansTest.Orleans.Common;

public class ClusterClientHostedService : IHostedService
{
	public IClusterClient ClusterClient { get; }

	public ClusterClientHostedService(IConfiguration configuration)
	{
		ClusterClient = new ClientBuilder().Configure<ClusterOptions>(options =>
			{
				options.ClusterId = "OrleansTest-host";
				options.ServiceId = "OrleansTest-host";
			})
			.UseAdoNetClustering(options =>
			{
				options.Invariant = "Npgsql";
				options.ConnectionString = configuration.GetValue<string>("POSTGRESQL_CONNECTION_STRING") ?? throw new NullReferenceException("POSTGRESQL_CONNECTION_STRING is null");
			})
			.Build();
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		return ClusterClient.Connect();
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		await using (ClusterClient)
		{
			await ClusterClient.Close();
		}
	}
}