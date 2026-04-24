using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Application.Dtos.Orders
{
    public class SapListDto <T>
    {
        public List<T> Value { get; set; } = new();
    }

    public class SapIEnumerableDto<T>
    {
        public List<T> Value { get; set; } = [];
        [JsonPropertyName("odata.count")]
        public int Count { get; set; }
    }
}
