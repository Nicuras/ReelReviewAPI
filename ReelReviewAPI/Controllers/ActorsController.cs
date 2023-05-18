using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReelReviewAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReelReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ReelRadarDbContext _context;

        public ActorsController(ReelRadarDbContext context)
        {
            _context = context;
        }

        // GET: api/actors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> GetActors()
        {
            return await _context.Actors.ToListAsync();
        }

        // GET: api/actors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Actor>> GetActor(long id)
        {
            var actor = await _context.Actors.FindAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            return actor;
        }

        // POST: api/actors
        [HttpPost]
        public async Task<ActionResult<Actor>> PostActor(Actor actor)
        {
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetActor), new { id = actor.ActorId }, actor);
        }

        // PUT: api/actors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActor(long id, Actor actor)
        {
            if (id != actor.ActorId)
            {
                return BadRequest();
            }

            _context.Entry(actor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(id))
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

        // DELETE: api/actors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(long id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActorExists(long id)
        {
            return _context.Actors.Any(e => e.ActorId == id);
        }
    }
}