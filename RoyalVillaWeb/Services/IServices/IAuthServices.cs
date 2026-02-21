using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoyalVilla.Dto;

namespace RoyalVillaWeb.Services.IServices
{
    public interface IAuthServices
    
    {
        Task<T?> LoginAsync<T>(LoginRequestDTO loginRequestDTO);
        Task<T?> RegisterAsync<T>(RegistrationRequestDTO registrationRequestDTO);
       
    }
}