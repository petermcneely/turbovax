using System.Text.Json.Serialization;

namespace TurboVax
{
    public record Portal
    {
        public string Name { get; set; }
        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }
        public string Key { get; set; }
        public string Url { get; set; }
        [JsonPropertyName("show_name_in_card")]
        public bool ShowNameInCard { get; set; }
        public string Type { get; set; }
    }
}
