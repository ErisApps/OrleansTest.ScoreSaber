// See https://aka.ms/new-console-template for more information

using OrleansTest.Orleans.Common;
using OrleansTest.ScoreSaber.LoadGenerator;

Console.WriteLine("Hello, World!");

var host = Host.CreateDefaultBuilder(args)
	.ConfigureAppConfiguration(builder => builder.AddEnvironmentVariables())
	.ConfigureServices(services =>
	{
		services.AddSingleton<ClusterClientHostedService>();
		services.AddHostedService(sp => sp.GetRequiredService<ClusterClientHostedService>());
		services.AddSingleton(sp => sp.GetRequiredService<ClusterClientHostedService>().ClusterClient);

		services.AddHostedService<WebSocketWorker>();
	})
	.Build();

await host.RunAsync();