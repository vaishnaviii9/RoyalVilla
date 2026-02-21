using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoyalVilla.Dto;

namespace RoyalVillaWeb.Services.IServices
{
    public interface IVillaServices
    {
        Task<T?> GetAllAsync<T>();
        Task<T?> GetAsync<T>(int id);
        Task<T?> CreateAsync<T>(VillaCreateDTO dto);
        Task<T?> UpdateAsync<T>(VillaUpdateDTO dto);
        Task<T?> DeleteAsync<T>(int id);
    }
}