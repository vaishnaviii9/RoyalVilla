using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services;
using RoyalVillaWeb.Services.IServices;
using AutoMapper;
using RoyalVilla.Dto;

namespace RoyalVillaWeb.Controllers;

public class HomeController : Controller
{
    private readonly IVillaServices _villaService;
    private readonly IMapper _mapper;

    public HomeController(IVillaServices villaServices, IMapper mapper)
    {
        _mapper = mapper;
        _villaService = villaServices;
    }

    // [Authorize]
    public async Task<IActionResult> Index()
    {
        List<VillaDTO> villaList = new();
        try
        {
            var response = await _villaService.GetAllAsync<ApiResponse<List<VillaDTO>>>();
            if(response!=null && response.Success && response.Data != null)
            {
                villaList = response.Data;
            }
        }
        catch (System.Exception ex)
        {
            TempData["error"] =$"An error occurred : {ex.Message}";
        }
        return View(villaList);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
