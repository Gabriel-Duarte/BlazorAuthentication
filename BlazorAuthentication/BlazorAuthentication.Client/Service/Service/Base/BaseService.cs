using System.Text.Json;
using System.Text;
using RestSharp;
namespace BlazorAuthentication.Client.Service.Service.Base
{
    public abstract class BaseService
    {

        protected StringContent SerializeObject(object data) =>
            new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json");

        protected string SerializeObjectToJson(object data) =>
             JsonSerializer.Serialize(data);

        protected async Task<T?> DeserializeObject<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected T? DeserializeObject<T>(RestResponse restResponse)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(restResponse.Content ?? "", options);
        }

        protected string RemoveBase64Header(string base64String)
        {
            int commaIndex = base64String.IndexOf(',');

            if (commaIndex != -1 && commaIndex + 1 < base64String.Length)
            {
                return base64String.Substring(commaIndex + 1);
            }

            return base64String;
        }
        protected bool AreAllPropertiesNull(object obj)
        {
            if (obj == null) return true;

            return obj.GetType().GetProperties()
                       .All(property => property.GetValue(obj) == null);
        }
    }
}
