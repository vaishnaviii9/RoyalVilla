using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services;
using RoyalVillaWeb.Services.IServices;
using AutoMapper;
using RoyalVilla.Dto;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace RoyalVillaWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthServices _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthServices authService, IMapper mapper)
        {
            _mapper = mapper;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var response = await _authService.LoginAsync<ApiResponse<LoginResponseDTO>>(loginRequestDTO);
                if (response != null && response.Success && response.Data != null)
                {
                    LoginResponseDTO model = response.Data;

                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(model.Token);

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u=> u.Type=="email").Value));
                    identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u=> u.Type=="role").Value));

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (System.Exception ex)
            {
                TempData["error"] = $"An error occurred : {ex.Message}";
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegistrationRequestDTO
            {
                Email = string.Empty,
                Name = string.Empty,
                Password = string.Empty,
                Role = "Customer"
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            try
            {
                ApiResponse<UserDTO> response = await _authService.RegisterAsync<ApiResponse<UserDTO>>(registrationRequestDTO);
                if (response != null && response.Success && response.Data != null)
                {
                    TempData["success"] = "Registration successful! Please login with your credentials.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["error"] = response?.Message?? "Registration failed. Please try again.";
                    return View(registrationRequestDTO);
                }
            }
            catch (System.Exception ex)
            {
                TempData["error"] = $"An error occurred : {ex.Message}";
            }
            return View(registrationRequestDTO);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

    }

}