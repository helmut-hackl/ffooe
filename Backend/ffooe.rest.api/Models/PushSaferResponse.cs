using System.Text.Json.Serialization;

namespace ffooe.rest.api.Models
{
    public class PushSaferResponse
    {
        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("answer")]
        public string Answer { get; set; }

        [JsonPropertyName("available")]
        public int Available { get; set; }

        [JsonPropertyName("group")]
        public string Group { get; set; }

        [JsonPropertyName("guest")]
        public string Guest { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("message_id")]
        public int MessageId { get; set; }

        [JsonPropertyName("message_ids")]
        public string MessageIds { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("success")]
        public string Success { get; set; }
    }
}
