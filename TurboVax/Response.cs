using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TurboVax
{
    public class Response
    {
        [JsonPropertyName("last_updated_at")]
        public DateTimeOffset? LastUpdatedAt { get; set; }
        public IEnumerable<Portal> Portals { get; set; }
        public IEnumerable<Location> Locations { get; set; }
    }
}
