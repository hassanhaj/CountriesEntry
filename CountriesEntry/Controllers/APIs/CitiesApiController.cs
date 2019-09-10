using CountriesEntry.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountriesEntry.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CitiesApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CitiesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            return await _context.Cities.Include(e => e.Country).ToListAsync();
        }

        // GET: api/CitiesApi/5
        [HttpGet("ByCountry/{countryId}")]
        public async Task<ActionResult<IEnumerable<City>>> GetByCountry(int countryId)
        {
            var cities = await _context.Cities
                .Where(e => e.CountryId == countryId)
                .ToListAsync();

            return cities;
        }

        // GET: api/CitiesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        // PUT: api/CitiesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(int id, City city)
        {
            if (id != city.Id)
            {
                return BadRequest();
            }

            _context.Entry(city).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CitiesApi
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(City city)
        {
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { id = city.Id }, city);
        }

        // DELETE: api/CitiesApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<City>> DeleteCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return city;
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
    }
}
