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
        public VillaServices(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
           
        }
        public Task<T?> GetAllAsync<T>(string token)
        {
           return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.GET,
                Url =APIEndpoint,
                Token = token
            });
        }

        public Task<T?> GetAsync<T>(int id, string token)
        {
           return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Url =$"{APIEndpoint}/{id}",
                Token = token
            });
        }
        public Task<T?> CreateAsync<T>(VillaCreateDTO dto,string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = dto,
                Url =APIEndpoint,
                Token = token
            });
        }
        public Task<T?> UpdateAsync<T>(VillaUpdateDTO dto, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = dto,
                Url = $"{APIEndpoint}/{dto.Id}",
                Token = token
            });
        }
        public Task<T?> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.DELETE,
                // Data = dto,
                Url = $"{APIEndpoint}/{id}",
                Token = token
            });
        }
    }
}