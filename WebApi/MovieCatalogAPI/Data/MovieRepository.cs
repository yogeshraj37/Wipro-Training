using MovieCatalogAPI.Models;

namespace MovieCatalogAPI.Data
{
    public static class MovieRepository
    {
        public static List<Director> Directors = new()
        {
            new Director { Id = 1, Name = "Yogesh Raj" },
            new Director { Id = 2, Name = "kya Rakha hai" }
        };

        public static List<Movie> Movies = new()
        {
            new Movie
            {
                Id = 1,
                Title = "Inception",
                DirectorId = 1,
                ReleaseYear = 2010
            },
            new Movie
            {
                Id = 2,
                Title = "Avatar",
                DirectorId = 2,
                ReleaseYear = 2009
            }
        };
    }
}