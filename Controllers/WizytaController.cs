using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Exceptions;
using WebApplication1.Models.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WizytaController : ControllerBase
    {
        private readonly IDbService _dbService;
        public WizytaController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{id}/visits")]
        public async Task<IActionResult> GetCustomerVisits(int id)
        {
            try
            {
                var res = await _dbService.getVisits(id);
                return Ok(res);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
        [HttpPost("{id}/rentals")]
        public async Task<IActionResult> AddNewRental(int id, GetVisits addVisits)
        {

            try
            {
                await _dbService.AddNewVisit(id, addVisits);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }

            return null;
        }    
    }
}
