using System.Linq;
using Newtonsoft.Json.Linq;

namespace CoreBot.Helper
{
    public class JsonUtil
    {
        public static string GetJsonValueByKey(JObject JsonArray, JToken key)
        {
            return JsonArray.Properties()?.FirstOrDefault(x => x?.Name == key?.ToString())?.Value?.ToString() ?? string.Empty;
        }
    }
}
