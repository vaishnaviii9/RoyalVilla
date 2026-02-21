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
            if(response!=null && response.Success && response.Data != null)
            {
                LoginResponseDTO model = response.Data;
            }
        }
        catch (System.Exception ex)
        {
            TempData["error"] =$"An error occurred : {ex.Message}";
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
            Role="Customer"
        });
    }

    public IActionResult AccessDenied()
    {
            return View();
    }
    public async Task<IActionResult> Logout()
    {
            return View();
    }

}

}