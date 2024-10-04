using System.IO;
using Newtonsoft.Json;
using RestSharp;

namespace PetStoreAutotests.Utilities
{
    public static class JsonParser
    {
        public static string Serialize<T>(T body)
        {
            return JsonConvert.SerializeObject(body, Formatting.Indented);
        }

        public static Entities ParseJson<Entities>(string file)
        {
            return file == null ? default : JsonConvert.DeserializeObject<Entities>(File.ReadAllText(file));
        }

        public static Entities ParseJson<Entities>(RestResponse response)
        {
            return response == null ? default : JsonConvert.DeserializeObject<Entities>(response.Content);
        }
    }
}