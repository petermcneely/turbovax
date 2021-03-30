using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TurboVax
{
    public class Scraper
    {
        public Scraper(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<AvailableAppointmentsResponse> ListAvailableAppointments(ListAvailableAppointmentsRequest request)
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetAsync(URL);
            var content = await JsonSerializer.DeserializeAsync<Response>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
            });

            var portalMap = content.Portals.ToDictionary(p => p.Key);
            foreach (var location in content.Locations)
            {
                location.Url = portalMap[location.Portal].Url;
            }
            return new AvailableAppointmentsResponse
            {
                Locations = content.Locations.Where(l =>
                    l.Active &&
                    (l.Available ?? false) &&
                    (!(request.PortalNames?.Any() ?? false) || request.PortalNames.Contains(portalMap[l.Portal].Name))),
            };
        }

        public const string URL = @"https://turbovax.global.ssl.fastly.net/dashboard";

        private readonly IHttpClientFactory httpClientFactory;
    }
}
