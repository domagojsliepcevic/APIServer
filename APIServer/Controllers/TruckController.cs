using APIServer.Data;
using APIServer.Interface;
using APIServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/truck")]
    [Produces("application/xml")]
    [Consumes("application/xml")]
    public class TruckController : ControllerBase
    {
        private readonly ITruckRepository _truckRepository;
        private readonly ICustomValidator _customValidator;

        public TruckController(ITruckRepository truckRepository, ICustomValidator customValidator)
        {
            _truckRepository = truckRepository;
            _customValidator = customValidator;
        }

        // GET: api/truck
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Truck>>> GetTrucks()
        {
            var trucks = await _truckRepository.GetAllTrucks();
            return Ok(trucks);
        }

        // GET: api/truck/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Truck>> GetTruck(int id)
        {
            var truck = await _truckRepository.GetTruckById(id);
            if (truck == null)
            {
                return NotFound();
            }
            return truck;
        }

        // POST: api/truck
        [HttpPost]
        public async Task<IActionResult> PostTruck([FromBody] Truck truck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Validate truck against RNG schema
                if (!_customValidator.ValidateTruck(truck))
                {
                    return BadRequest("Invalid truck data.");
                }

                await _truckRepository.AddTruck(truck);
                return CreatedAtAction(nameof(GetTruck), new { id = truck.Id }, truck);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // PUT: api/truck/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTruck(int id, [FromBody] Truck truck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Set the ID of the truck object
                truck.Id = id;

                // Validate truck against RNG schema
                if (!_customValidator.ValidateTruck(truck))
                {
                    return BadRequest("Invalid truck data.");
                }

                // Update the truck in the repository
                await _truckRepository.UpdateTruck(truck);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // DELETE: api/truck/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTruck(int id)
        {
            try
            {
                await _truckRepository.DeleteTruck(id);
                return Ok("Truck deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
