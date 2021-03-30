
using Newtonsoft.Json;

namespace PolygonProp.PolygonApi.DtoModels
{
    public class Polygon
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Data")]
        public string Data { get; set; }
        [JsonProperty("Wkt")]
        public string Wkt { get; set; }
    }
}
