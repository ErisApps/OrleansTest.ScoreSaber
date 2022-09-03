// See https://aka.ms/new-console-template for more information

using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;
using Orleans.ApplicationParts;
using Orleans.Hosting;
using OrleansTest.ScoreSaber.Common.Grains.Interfaces;
using OrleansTest.ScoreSaber.Silo.Grains;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseOrleans(static (context, siloBuilder) =>
{
    // siloBuilder.ConfigureApplicationParts(manager => manager.AddApplicationPart(new AssemblyPart(typeof(ICountryGrain).Assembly)));
    siloBuilder.ConfigureServices(collection =>
    {
        collection.AddTransient<ICountryGrain, CountryGrain>();
        collection.AddTransient<IPlayerGrain, PlayerGrain>();
        collection.AddTransient<ITestGrain, TestGrain>();
    });

    siloBuilder.UseLocalhostClustering();
});

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