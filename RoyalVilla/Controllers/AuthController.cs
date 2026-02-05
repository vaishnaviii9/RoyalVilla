using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla.Models.DTO;

namespace RoyalVilla.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {
            
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaDTO>>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<object>>),StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<UserDTO>>> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            //auth service

            

            var response = ApiResponse<UserDTO>.Ok(null, "User created successfully");

            return Ok(response);
        }

    }
}