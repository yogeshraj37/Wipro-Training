using Microsoft.AspNetCore.Mvc;
using MovieCatalogAPI.Data;
using MovieCatalogAPI.Models;

namespace MovieCatalogAPI.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        // GET: api/movies
        // Returns all movies from the in-memory collection
        [HttpGet]
        public IActionResult GetMovies()
        {
            return Ok(MovieRepository.Movies);
        }

        // GET: api/movies/{id}
        // Returns a specific movie using its ID
        [HttpGet("{id}")]
        public IActionResult GetMovie(int id)
        {
            var movie = MovieRepository.Movies
                .FirstOrDefault(x => x.Id == id);

            if (movie == null)
                return NotFound("Movie not found");

            return Ok(movie);
        }

        // POST: api/movies
        // Creates a new movie
        // Request Body: JSON movie object
        [HttpPost]
        public IActionResult CreateMovie(Movie movie)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            movie.Id = MovieRepository.Movies.Max(x => x.Id) + 1;

            MovieRepository.Movies.Add(movie);

            return CreatedAtAction(
                nameof(GetMovie),
                new { id = movie.Id },
                movie);
        }

        // PUT: api/movies/{id}
        // Updates an existing movie using its ID
        [HttpPut("{id}")]
        public IActionResult UpdateMovie(int id, Movie updatedMovie)
        {
            var movie = MovieRepository.Movies
                .FirstOrDefault(x => x.Id == id);

            if (movie == null)
                return NotFound("Movie not found");

            movie.Title = updatedMovie.Title;
            movie.DirectorId = updatedMovie.DirectorId;
            movie.ReleaseYear = updatedMovie.ReleaseYear;

            return Ok(movie);
        }

        // DELETE: api/movies/{id}
        // Deletes a movie using its ID
        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            var movie = MovieRepository.Movies
                .FirstOrDefault(x => x.Id == id);

            if (movie == null)
                return NotFound("Movie not found");

            MovieRepository.Movies.Remove(movie);

            return NoContent();
        }
    }
}