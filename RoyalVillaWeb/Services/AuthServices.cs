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
        private const string APIEndpoint = "/api/villa";
        public AuthServices(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
           
        }

        public Task<T?> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }
        public Task<T?> RegisterAsync<T>(RegisterRequestDTO registerRequestDTO)
        {
            throw new NotImplementedException();
        }
}
}