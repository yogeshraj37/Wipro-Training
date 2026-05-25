using Microsoft.AspNetCore.Mvc;
using MovieCatalogAPI.Data;
using MovieCatalogAPI.Models;

namespace MovieCatalogAPI.Controllers
{
    [ApiController]
    [Route("api/directors")]
    public class DirectorsController : ControllerBase
    {
        // GET: api/directors
        // Returns all directors
        [HttpGet]
        public IActionResult GetDirectors()
        {
            return Ok(MovieRepository.Directors);
        }

        // GET: api/directors/{id}
        // Returns a specific director by ID
        [HttpGet("{id}")]
        public IActionResult GetDirector(int id)
        {
            var director = MovieRepository.Directors
                .FirstOrDefault(x => x.Id == id);

            if (director == null)
                return NotFound("Director not found");

            return Ok(director);
        }

        // POST: api/directors
        // Creates a new director
        [HttpPost]
        public IActionResult CreateDirector(Director director)
        {
            director.Id = MovieRepository.Directors.Max(x => x.Id) + 1;

            MovieRepository.Directors.Add(director);

            return CreatedAtAction(
                nameof(GetDirector),
                new { id = director.Id },
                director);
        }

        // GET: api/directors/{directorId}/movies
        // Returns all movies directed by a specific director
        // Demonstrates association between Director and Movie
        [HttpGet("{directorId}/movies")]
        public IActionResult GetMoviesByDirector(int directorId)
        {
            var director = MovieRepository.Directors
                .FirstOrDefault(x => x.Id == directorId);

            if (director == null)
                return NotFound("Director not found");

            var movies = MovieRepository.Movies
                .Where(x => x.DirectorId == directorId);

            return Ok(movies);
        }
    }
}