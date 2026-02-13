using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoyalVilla.Dto;

namespace RoyalVilla.Services
{
    public interface IAuthService
    {
       Task<UserDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);
        Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<bool> IsEmailExistsAsync(string email);

    }
}