using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla.Data;
using RoyalVilla.Models;
using RoyalVilla.Dto;
using Asp.Versioning;

namespace RoyalVilla.Controllers.v2
{
    [Route("api/v{version:apiVersion}/villa-amenities")]
    // [ApiExplorerSettings(GroupName ="v1")]
    [ApiVersion("2.0")]
    [ApiController]
    public class VillaAmenitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaAmenitiesController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaAmenitiesDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<object>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<VillaAmenitiesDTO>>>> GetVillaAmenities()
        {
            var villaAmenities = await _db.VillaAmenities.ToListAsync();

            var dtoResponseVillaAmenities = _mapper.Map<List<VillaAmenitiesDTO>>(villaAmenities);
            var response = ApiResponse<IEnumerable<VillaAmenitiesDTO>>.Ok(dtoResponseVillaAmenities, "Villa amenities retrieved successfully");

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<object>>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<object>>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> GetVillaAmenitiesById(int id)
        {
            try
            {
                if (id == 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("Villa Amenities Id must be greater than zero"));
                }

                var villaAmenities = await _db.VillaAmenities.FirstOrDefaultAsync(u => u.Id == id);

                if (villaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa Amenities with Id: {id} not found"));
                }

                return Ok(ApiResponse<VillaAmenitiesDTO>.Ok(_mapper.Map<VillaAmenitiesDTO>(villaAmenities), "Records retrieved successfully"));
            }

            catch (System.Exception ex)
            {

                var response = ApiResponse<object>.Error(500, $"An error occurred while creating the villa amenities: {ex.Message} ", ex.Message);
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<object>>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<object>>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<object>>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> CreateVillaAmenities(VillaAmenitiesCreateDTO villaAmenitiesDTO)
        {
            try
            {
                if (villaAmenitiesDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa Amenities data is required"));
                }

                var villaExists = await _db.Villa.FirstOrDefaultAsync(u => u.Id == villaAmenitiesDTO.VillaId);

                if (villaExists == null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"Villa with Id '{villaAmenitiesDTO.VillaId}' doesnot exists"));
                }

                VillaAmenities villaAmenities = _mapper.Map<VillaAmenities>(villaAmenitiesDTO);
                villaAmenities.CreatedDate = DateTime.Now;
                await _db.VillaAmenities.AddAsync(villaAmenities);
                await _db.SaveChangesAsync();

                var response = ApiResponse<VillaAmenitiesDTO>.CreatedAt(_mapper.Map<VillaAmenitiesDTO>(villaAmenities), "Villa Amenities created successfully");
                return CreatedAtAction(nameof(CreateVillaAmenities), new { id = villaAmenities.Id }, response);
            }
            catch (System.Exception ex)
            {

                var response = ApiResponse<object>.Error(500, $"An error occurred while retrieving the villa amenities: {ex.Message} ", ex.Message);
                return StatusCode(500, response);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<object>>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<object>>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<object>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<object>>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> UpdateVillaAmenities(int id, VillaAmenitiesUpdateDTO villaAmenitiesDTO)
        {
            try
            {
                if (villaAmenitiesDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa Amenities data is required"));
                }

                if (id != villaAmenitiesDTO.Id)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa Amenities ID in URL does not match Villa Amenities ID in request body"));
                }
                var villaExists = await _db.Villa.FirstOrDefaultAsync(u => u.Id == villaAmenitiesDTO.VillaId);

                if (villaExists == null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"Villa with Id '{villaAmenitiesDTO.VillaId}' doesnot exists"));
                }
                var existingVillaAmenities = await _db.VillaAmenities.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVillaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa Amenities with ID {id} was not found"));
                }

                _mapper.Map(villaAmenitiesDTO, existingVillaAmenities);
                existingVillaAmenities.UpdatedDate = DateTime.Now;

                await _db.SaveChangesAsync();

                var response = ApiResponse<VillaAmenitiesDTO>.Ok(_mapper.Map<VillaAmenitiesDTO>(existingVillaAmenities), "Villa amenities updated successfully");
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                var response = ApiResponse<object>.Error(500, $"An error occurred while updating the villa amenities: {ex.Message} ", ex.Message);
                return StatusCode(500, response);
            }
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<object>>),StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<object>>),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVillaAmenities(int id)
        {
            try
            {

                var existingVillaAmenities = await _db.VillaAmenities.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVillaAmenities == null)
                {

                    return NotFound(ApiResponse<object>.NotFound($"Villa Amenities with ID {id} was not found"));
                }

                _db.VillaAmenities.Remove(existingVillaAmenities);
                await _db.SaveChangesAsync();

                var response = ApiResponse<object>.NoContent("Villa Amenities deleted successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<object>.Error(500, $"An error occurred while deleting the villa amenities : {ex.Message}", ex.Message);
                return StatusCode(500, response);
            }
        }
    
    }

}