using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace TurboVax
{
    public static class Program
    {
        public static async Task Main(string[] args) =>
            await new Parser(settings =>
            {
                settings.HelpWriter = Console.Error;
                settings.EnableDashDash = true;
            })
            .ParseArguments<TurboVaxOptions>(args)
            .WithParsedAsync(async options =>
                await new HostBuilder()
                    .ConfigureAppConfiguration(builder =>
                    {
                        builder
                            .AddJsonFile("appsettings.json")
                            .AddUserSecrets(typeof(Program).Assembly);
                    })
                    .ConfigureServices((hc, serviceCollection) =>
                    {
                        serviceCollection
                            .AddHttpClient()
                            .AddTransient<Scraper>()
                            .AddTransient<Texter>()
                            .AddHostedService<ScrapeService>()
                            .AddLogging(loggingBuilder =>
                            {
                                loggingBuilder
                                    .AddConsole()
                                    .SetMinimumLevel(LogLevel.Error)
                                    .AddFilter("TurboVax", LogLevel.Information);
                            })
                            .AddSingleton(options);
                    })
                    .UseConsoleLifetime()
                    .Build()
                    .RunAsync());
    }
}
