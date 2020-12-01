using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouletteBets_WebApp.DataAccess;
using RouletteBets_WebApp.Models;

namespace RouletteBets_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoulettesController : ControllerBase
    {
        private readonly RouletteBetsContext _context;

        public RoulettesController(RouletteBetsContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetRoulettes")]
        public async Task<ActionResult<IEnumerable<Roulette>>> GetRoulettes()
        {
            return await _context.Roulette.ToListAsync();
        }

        [HttpPut]
        [Route("OpenRoulette/{id}")]
        public async Task<IActionResult> OpenRoulette(long id)
        {
            Roulette roulette = await _context.Roulette.FindAsync(id);
            roulette.IsOpen = true;
            _context.Entry(roulette).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RouletteExists(id))
                {
                    return NotFound(false);
                }
                else
                {
                    throw;
                }
            }

            return Ok(true);
        }


        [HttpPost]
        [Route("CreateRoulette")]
        public async Task<IActionResult> CreateRoulette(Roulette roulette)
        {
            roulette.Code = string.IsNullOrWhiteSpace(roulette.Code) ? "" : roulette.Code.ToUpper().Trim(); 
            _context.Roulette.Add(roulette);
            await _context.SaveChangesAsync();

            return Ok(roulette.Id);
        }

        private bool RouletteExists(long id)
        {
            return _context.Roulette.Any(e => e.Id == id);
        }
    }
}
