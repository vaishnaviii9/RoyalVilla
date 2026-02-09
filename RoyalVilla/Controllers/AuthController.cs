using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla.Models.DTO;
using RoyalVilla.Services;

namespace RoyalVilla.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authservice = authService;

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<UserDTO>>> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            try
            {
                //auth service
                if (registrationRequestDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registration data is required"));

                }

                if (await _authservice.IsEmailExistsAsync(registrationRequestDTO.Email))
                {
                    return Conflict(ApiResponse<object>.Conflict($"User with email '{registrationRequestDTO.Email} already exists'"));
                }

                var user = await _authservice.RegisterAsync(registrationRequestDTO);

                if (user == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registration failed"));

                }

                var response = ApiResponse<UserDTO>.CreatedAt(user, "User created successfully");

                return CreatedAtAction(nameof(Register), response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<object>.Error(500, $"An error occurred during registration : {ex.Message}", ex.Message);
                return StatusCode(500, response);
            }
        }


        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LoginResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<LoginRequestDTO>>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                //auth service
                if (loginRequestDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Login data is required"));

                }

               
                var loginResponse = await _authservice.LoginAsync(loginRequestDTO);

                if (loginResponse == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Login failed"));

                }

                var response = ApiResponse<LoginResponseDTO>.Ok(loginResponse, "Login registered successfully");

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<object>.Error(500, $"An error occurred during login : {ex.Message}", ex.Message);
                return StatusCode(500, response);
            }
        }

    }
}