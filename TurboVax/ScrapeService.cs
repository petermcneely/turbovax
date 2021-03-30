using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TurboVax
{
    public class ScrapeService : BackgroundService
    {
        public ScrapeService(
            Scraper scraper,
            IConfiguration configuration,
            ILogger<ScrapeService> logger,
            TurboVaxOptions options,
            Texter texter)
        {
            this.scraper = scraper;
            this.configuration = configuration;
            this.logger = logger;
            this.texter = texter;
            this.options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var portalNames = options.PortalNames?.Split('|').ToHashSet();
                    var response = await scraper.ListAvailableAppointments(new ListAvailableAppointmentsRequest
                    {
                        PortalNames = portalNames
                    });
                    if (response.Locations?.Any() ?? false)
                    {
                        var stringBuilder = new StringBuilder();
                        foreach (var location in response.Locations)
                        {
                            stringBuilder.AppendLine(location.ToString());
                        }
                        var serializedLocations = stringBuilder.ToString();
                        var textTask = texter.Text(serializedLocations);
                        logger.LogInformation(serializedLocations);
                        await textTask;
                        if (options.FindAndQuit)
                        {
                            logger.LogInformation("Task completed");
                            return;
                        }
                    }
                    else
                    {
                        logger.LogDebug("Did not find an appointment -- will try again");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Encountered an error while scraping");
                }
                finally
                {
                    await Task.Delay(configuration.GetValue<TimeSpan>("DELAY"), stoppingToken);
                }
            }
        }

        private readonly Scraper scraper;
        private readonly IConfiguration configuration;
        private readonly ILogger logger;
        private readonly Texter texter;
        private readonly TurboVaxOptions options;
    }
}
