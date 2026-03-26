using System.Text.Json;

namespace WebFrontEnd.Services
{
    public static class General
    {
        public static string SerializeObject(object modelObject) => JsonSerializer.Serialize(modelObject, JsonOptions());
        public static T DeserialzeJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString, JsonOptions())!;
        public static StringContent GennerateStringContent(string serialiazedObj) => new(serialiazedObj, System.Text.Encoding.UTF8, "application/json");
        public static List<T> DeserialeJsonStringList<T>(string jsonString) => JsonSerializer.Deserialize<List<T>>(jsonString, JsonOptions())!.ToList();
        public static JsonSerializerOptions JsonOptions()
        {
            return new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Skip
            };
        }
    }
}

