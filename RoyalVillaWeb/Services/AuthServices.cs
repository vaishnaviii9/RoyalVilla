using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoyalVillaWeb.Services.IServices;
using RoyalVilla.Dto;
using RoyalVillaWeb.Models;

namespace RoyalVillaWeb.Services
{
    public class AuthServices: BaseServices, IAuthServices
    {
        private const string APIEndpoint = "/api/auth";
        public AuthServices(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
           
        }

        public Task<T?> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            
            return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = loginRequestDTO,
                Url =APIEndpoint + "/login",
                Token = null
            });
        }
        public Task<T?> RegisterAsync<T>(RegistrationRequestDTO registrationRequestDTO)
        {
           return SendAsync<T>(new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = registrationRequestDTO,
                Url =APIEndpoint + "/register",
                Token = null
            });
        }
}
}