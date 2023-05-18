using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReelReviewAPI.Models;

namespace ReelReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly ReelRadarDbContext _context;

        public DirectorsController(ReelRadarDbContext context)
        {
            _context = context;
        }

        // GET: api/Directors
        [HttpGet]
        public async Task<IActionResult> GetDirectors()
        {
            return _context.Directors != null ?
                        Ok(await _context.Directors.ToListAsync()) :
                        Problem("Entity set 'ReelRadarDbContext.Directors'  is null.");
        }

        // GET: api/Directors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDirector(long id)
        {
            if (_context.Directors == null)
            {
                return NotFound();
            }

            var director = await _context.Directors.FindAsync(id);
            if (director == null)
            {
                return NotFound();
            }

            return Ok(director);
        }

        // PUT: api/Directors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDirector(long id, Director director)
        {
            if (_context.Directors == null || id != director.DirectorId)
            {
                return BadRequest();
            }

            _context.Entry(director).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectorExists(id))
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

        // POST: api/Directors
        [HttpPost]
        public async Task<ActionResult<Director>> PostDirector(Director director)
        {
            if (ModelState.IsValid)
            {
                _context.Directors.Add(director);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetDirector), new { id = director.DirectorId }, director);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Directors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirector(long id)
        {
            if (_context.Directors == null)
            {
                return Problem("Entity set 'ReelRadarDbContext.Directors'  is null.");
            }

            var director = await _context.Directors.FindAsync(id);
            if (director == null)
            {
                return NotFound();
            }

            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DirectorExists(long id)
        {
            return (_context.Directors?.Any(e => e.DirectorId == id)).GetValueOrDefault();
        }
    }
}