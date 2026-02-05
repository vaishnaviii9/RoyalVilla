using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoyalVilla.Data;
using RoyalVilla.Models;
using RoyalVilla.Models.DTO;

namespace RoyalVilla.Services
{
    public class AuthService : IAuthService
    {

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public AuthService(ApplicationDbContext db, IConfiguration configuration, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _db.Users.
            AnyAsync(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }

        public Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            try
            {
                if (await IsEmailExistsAsync(registrationRequestDTO.Email))
                {
                    throw new InvalidOperationException($"User with email '{registrationRequestDTO.Email}' already exists");
                }
                User user = new()
                {
                    Email = registrationRequestDTO.Email,
                    Name = registrationRequestDTO.Name,
                    Password = registrationRequestDTO.Password,
                    Role = string.IsNullOrEmpty(registrationRequestDTO.Role) ? "Customer" : registrationRequestDTO.Role,
                    CreatedDate = DateTime.Now
                };

                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("An unexpected error occurred during user registration", ex);
                ;
            }
        }
    }
}