using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoyalVillaWeb.Services.IServices;
using RoyalVilla.Dto;
using RoyalVillaWeb.Models;

namespace RoyalVillaWeb.Services
{
    public class VillaServices: BaseServices, IVillaServices
    {
        private const string APIEndpoint = "/api/villa";
        public VillaServices(IHttpClientFactory httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor)
        {
           
        }
        public Task<T?> GetAllAsync<T>()
        {
           return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.GET,
                Url =APIEndpoint
            });
        }

        public Task<T?> GetAsync<T>(int id)
        {
           return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.GET,
                Url =$"{APIEndpoint}/{id}"
            });
        }
        public Task<T?> CreateAsync<T>(VillaCreateDTO dto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = dto,
                Url =APIEndpoint
            });
        }
        public Task<T?> UpdateAsync<T>(VillaUpdateDTO dto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = dto,
                Url = $"{APIEndpoint}/{dto.Id}"
            });
        }
        public Task<T?> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.DELETE,
                // Data = dto,
                Url = $"{APIEndpoint}/{id}"
            });
        }
    }
}