using Newtonsoft.Json;

namespace StarterApi.ApiModels.Middlewares
{
    public class ExceptionResponse
    {

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }


        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }


        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }


        [JsonProperty(PropertyName = "traceId")]
        public string TraceId { get; set; }


        [JsonProperty(PropertyName = "errors")]
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}
