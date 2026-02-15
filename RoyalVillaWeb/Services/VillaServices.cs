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
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _villaUrl;
        private const string APIEndpoint ="/api/villa";
        public VillaServices(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _clientFactory = httpClient;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }
        public Task<T?> GetAllAsync<T>(string token)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetAsync<T>(int id, string token)
        {
            throw new NotImplementedException();
        }
        public Task<T?> CreateAsync<T>(VillaCreateDTO dto,string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = dto,
                Url =$"{_villaUrl}{APIEndpoint}",
                Token = token
            });
        }
        public Task<T?> UpdateAsync<T>(VillaUpdateDTO dto, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = dto,
                Url = $"{_villaUrl}{APIEndpoint}/{dto.Id}",
                Token = token
            });
        }
        public Task<T?> DeleteAsync<T>(int id, string token)
        {
            throw new NotImplementedException();
        }
    }
}