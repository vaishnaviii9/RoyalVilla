using System.Net.Http.Json;
using System.Text.Json;
using RoyalVilla.DTO;
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
       public Task<T?> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("RoyalVillaAPI");
                var message = new HttpRequestMessage
                {
                    RequestUri = new Uri(apiRequest.Url),
                    Method = GetHttpMethod(apiRequest.ApiType),
                };

                if(apiRequest.Data != null)
                {
                   message.Content = JsonContent.Create(apiRequest.Data, options: JsonOptions); 
                }

                var apiResponse = await client.SendAsync(message);

                return await apiResponse.Content.ReadFromJsonAsync<T>(JsonOptions);

            }
            catch (System.Exception ex)
            {
                
                Console.WriteLine($"Unexpected error : {ex.Message}");
                return default;

            }
        }

        private static HttpMethod GetHttpMethod(StaticDetails.ApiType apiType)
        {
            return apiType switch
            {
              StaticDetails.ApiType.POST => HttpMethod.POST,
              StaticDetails.ApiType.PUT => HttpMethod.PUT,
              StaticDetails.ApiType.DELETE => HttpMethod.DELETE,
              _ => HttpMethod.GET,
            };
        }
    }
}