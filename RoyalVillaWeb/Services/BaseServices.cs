using System.Net.Http.Json;
using System.Text.Json;
using RoyalVilla.Dto;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;

namespace RoyalVillaWeb.Services
{
    public class BaseServices : IBaseServices
    {

        public IHttpClientFactory _httpClient {get; set;}

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiResponse<object> ResponseModel {get; set;}

        public BaseServices(IHttpClientFactory httpClient)
        {
            this.ResponseModel = new();
            _httpClient = httpClient;
        }
       public async Task<T?> SendAsync<T>(ApiRequest apiRequest)
        {
            var client = _httpClient.CreateClient("RoyalVillaAPI");
            if (string.IsNullOrEmpty(apiRequest.Url))
                throw new ArgumentNullException(nameof(apiRequest.Url), "API request URL cannot be null or empty.");

            var message = new HttpRequestMessage
            {
                RequestUri = new Uri(apiRequest.Url, uriKind: UriKind.Relative),
                Method = GetHttpMethod(apiRequest.ApiType),
            };

            if (apiRequest.Data != null)
            {
                message.Content = JsonContent.Create(apiRequest.Data, options: JsonOptions);
            }

            if (!string.IsNullOrEmpty(apiRequest.Token))
            {
                message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiRequest.Token);
            }

            var apiResponse = await client.SendAsync(message);

            return await apiResponse.Content.ReadFromJsonAsync<T>(JsonOptions);
        }

        private static HttpMethod GetHttpMethod(StaticDetails.ApiType apiType)
        {
            return apiType switch
            {
              StaticDetails.ApiType.POST => HttpMethod.Post,
              StaticDetails.ApiType.PUT => HttpMethod.Put,
              StaticDetails.ApiType.DELETE => HttpMethod.Delete,
              _ => HttpMethod.Get,
            };
        }
    }
}