using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Uplift.Extensions
{
    public static class SessionExtensions
    {
        public static bool IsExist(this ISession session, string key) =>
            session.GetString(key) != null;
        public static void SetObject(this ISession session, string key, object value) =>
            session.SetString(key, JsonConvert.SerializeObject(value));

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}