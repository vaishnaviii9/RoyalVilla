using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoyalVilla.DTO;
using RoyalVillaWeb.Models;

namespace RoyalVillaWeb.Services.IServices
{
    public interface IBaseServices
    {
        ApiResponse<object> ResponseModel {get; set;}
        Task<T> SendAsync<T>(ApiRequest apiRequest); 
    }
}