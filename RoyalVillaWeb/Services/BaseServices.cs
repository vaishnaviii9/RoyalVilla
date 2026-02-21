using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using RoyalVilla.Dto;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;

namespace RoyalVillaWeb.Services
{
    public class BaseServices : IBaseServices
    {

        public IHttpClientFactory _httpClient { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiResponse<object> ResponseModel { get; set; }

        public BaseServices(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor)
        {
            this.ResponseModel = new();
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
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

            var token = _httpContextAccessor.HttpContext?.Session?.GetString(StaticDetails.SessionToken);
            if(!string.IsNullOrEmpty(token))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            if (apiRequest.Data != null)
            {
                message.Content = JsonContent.Create(apiRequest.Data, options: JsonOptions);
            }

            if (!string.IsNullOrEmpty(apiRequest.Token))
            {
                message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiRequest.Token);
            }

            var apiResponse = await client.SendAsync(message);

            // Check if response was successful
            if (!apiResponse.IsSuccessStatusCode)
            {
                var errorContent = await apiResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error {apiResponse.StatusCode}: {errorContent}");
                return default;
            }

            // Check if body is empty
            var content = await apiResponse.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
            {
                Console.WriteLine("API returned empty response body.");
                return default;
            }

            return JsonSerializer.Deserialize<T>(content, JsonOptions);
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