using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoyalVilla.Models.DTO;

namespace RoyalVilla.Services
{
    public class AuthService : IAuthService
    {
        public Task<bool> IsEmailExistsAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}