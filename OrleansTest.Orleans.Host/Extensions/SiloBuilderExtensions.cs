using Orleans.Hosting;

namespace OrleansTest.Orleans.Host.Extensions;

public static class SiloBuilderExtensions
{
	public static ISiloBuilder AddAdoNetStorage(this ISiloBuilder siloBuilder, string name, string connectionString)
	{
		return siloBuilder.AddAdoNetGrainStorage(
			name, configureOptions: options =>
			{
				options.Invariant = "Npgsql";
				options.ConnectionString = connectionString;
				options.UseJsonFormat = true;
			});
	}
}