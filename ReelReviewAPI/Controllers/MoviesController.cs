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
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly ReelRadarDbContext _context;

        public MoviesController(ReelRadarDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all movies.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var reelRadarDbContext = _context.Movies.Include(m => m.Director);
            return Ok(await reelRadarDbContext.ToListAsync());
        }

        /// <summary>
        /// Get a movie by ID.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(long id)
        {
            var movie = await _context.Movies
                .Include(m => m.Director)
                .FirstOrDefaultAsync(m => m.MovieId == id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        /// <summary>
        /// Add a new movie.
        /// </summary>
        /// <param name="movie">The movie to add.</param>
        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetMovie), new { id = movie.MovieId }, movie);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Update a movie.
        /// </summary>
        /// <param name="id">The ID of the movie to update.</param>
        /// <param name="movie">The updated movie.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(long id, [FromBody] Movie movie)
        {
            if (id != movie.MovieId)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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

        /// <summary>
        /// Delete a movie.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(long id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(long id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}