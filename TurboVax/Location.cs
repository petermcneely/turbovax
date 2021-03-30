using System;
using System.Text;
using System.Text.Json.Serialization;

namespace TurboVax
{
    public record Location
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool? Available { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }
        [JsonPropertyName("last_available_at")]
        public DateTimeOffset? LastAvailableAt { get; set; }
        public string Portal { get; set; }
        public string Area { get; set; }
        public Appointments Appointments { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Name: {Name}");
            builder.AppendLine($"Updated At: {UpdatedAt?.ToOffset(TimeSpan.FromHours(-4))}");
            builder.AppendLine($"Last Available At: {LastAvailableAt?.ToOffset(TimeSpan.FromHours(-4))}");
            builder.AppendLine($"Area: {Area}");
            builder.AppendLine($"Url: {Url}");
            return builder.ToString();
        }
    }
}
