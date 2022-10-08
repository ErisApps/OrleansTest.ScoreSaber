// See https://aka.ms/new-console-template for more information

using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Statistics;
using OrleansTest.Orleans.Host.Extensions;

await Host.CreateDefaultBuilder(args)
	.ConfigureAppConfiguration(configure =>
	{
		configure.AddEnvironmentVariables();
	})
	.UseOrleans(static (context, siloBuilder) =>
	{
		var instanceId = context.Configuration.GetValue<int?>("ORLEANS_INSTANCE_ID") ?? throw new NullReferenceException("ORLEANS_INSTANCE_ID is null");
		var postgreSqlConnectionString = context.Configuration.GetValue<string>("POSTGRESQL_CONNECTION_STRING") ?? throw new NullReferenceException("POSTGRESQL_CONNECTION_STRING is null");

#if DEBUG
		siloBuilder.ConfigureEndpoints(siloPort: 11111 + instanceId, gatewayPort: 30001);
		siloBuilder.UsePerfCounterEnvironmentStatistics();
#else
        siloBuilder.UseKubernetesHosting();
        siloBuilder.UseLinuxEnvironmentStatistics();
#endif

		siloBuilder.Configure<ClusterOptions>(options =>
		{
			options.ClusterId = "OrleansTest-host";
			options.ServiceId = "OrleansTest-host";
		});

		siloBuilder.UseAdoNetClustering(options =>
		{
			options.Invariant = "Npgsql";
			options.ConnectionString = postgreSqlConnectionString;
		});

		siloBuilder.AddAdoNetStorage("countryStore", postgreSqlConnectionString);
		siloBuilder.AddAdoNetStorage("playerStore", postgreSqlConnectionString);

		siloBuilder.Configure<GrainCollectionOptions>(o =>
		{
			o.CollectionAge = TimeSpan.FromMinutes(1);
			o.CollectionQuantum = TimeSpan.FromSeconds(30);
		});

		if (instanceId == 1)
		{
			siloBuilder.UseDashboard();
		}
	})
	.Build()
	.RunAsync();