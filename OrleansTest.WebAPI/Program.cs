using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;
using OrleansTest.Orleans.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSingleton<ClusterClientHostedService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<ClusterClientHostedService>());
builder.Services.AddSingleton(sp => sp.GetRequiredService<ClusterClientHostedService>().ClusterClient);

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddResponseCompression(static options =>
{
	options.EnableForHttps = true;
	options.Providers.Add<BrotliCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(static options => options.Level = CompressionLevel.Optimal);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP Request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.EnableTryItOutByDefault();
		options.DefaultModelsExpandDepth(1);
	});
}

app.UseHttpsRedirection();

app.UseResponseCompression();

app.UseAuthorization();

app.MapControllers();

app.Run();