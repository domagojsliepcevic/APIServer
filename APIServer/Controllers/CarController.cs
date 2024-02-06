// CarController.cs
using APIServer.Data;
using APIServer.Interface;
using APIServer.Models;
using APIServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace APIServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/car")]
    [Produces("application/xml")]
    [Consumes("application/xml")]
    public class CarController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly XmlValidator _xmlValidator;

        public CarController(ICarRepository carRepository, XmlValidator xmlValidator)
        {
            _carRepository = carRepository;
            _xmlValidator = xmlValidator;
        }

        // GET: api/car
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            var cars = await _carRepository.GetAllCars();
            return Ok(cars);
        }

        // GET: api/car/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _carRepository.GetCarById(id);
            if (car == null)
            {
                return NotFound();
            }
            return car;
        }

        // POST: api/car
        [HttpPost]
        public async Task<IActionResult> PostCar()
        {
            try
            {
                // Read XML data from request body
                string xmlData;
                using (StreamReader reader = new StreamReader(Request.Body))
                {
                    xmlData = await reader.ReadToEndAsync();
                }

                // Validate XML against XSD schema
                if (!_xmlValidator.ValidateXml(xmlData))
                {
                    return BadRequest("Invalid XML data.");
                }

                // Deserialize XML data into Car object
                Car car;
                XmlSerializer serializer = new XmlSerializer(typeof(Car));
                using (StringReader stringReader = new StringReader(xmlData))
                {
                    car = (Car)serializer.Deserialize(stringReader);
                }

                // Create XmlData property from request body
                car.XmlData = xmlData;

                // Check if any required properties are missing
                if (string.IsNullOrWhiteSpace(car.Make) || string.IsNullOrWhiteSpace(car.Model) || car.Year == 0 || string.IsNullOrWhiteSpace(car.Color))
                {
                    return BadRequest("One or more required properties are missing.");
                }

                // Save the car to the database
                await _carRepository.AddCar(car);
                return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // PUT: api/car/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id)
        {
            try
            {
                // Read XML data from request body
                string xmlData;
                using (StreamReader reader = new StreamReader(Request.Body))
                {
                    xmlData = await reader.ReadToEndAsync();
                }

                // Validate XML against XSD schema
                if (!_xmlValidator.ValidateXml(xmlData))
                {
                    return BadRequest("Invalid XML data.");
                }

                // Deserialize XML data into Car object
                Car car;
                XmlSerializer serializer = new XmlSerializer(typeof(Car));
                using (StringReader stringReader = new StringReader(xmlData))
                {
                    car = (Car)serializer.Deserialize(stringReader);
                }

                // Set the ID of the car object
                car.Id = id;

                // Create XmlData property from request body
                car.XmlData = xmlData;

                // Update the car in the repository
                await _carRepository.UpdateCar(car);

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


        // DELETE: api/car/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                await _carRepository.DeleteCar(id);
                return Ok("Car deleted successfully.");
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
