using KakYaNenavizhuEtotTutorialAAA.Models;
using Microsoft.EntityFrameworkCore;

namespace KakYaNenavizhuEtotTutorialAAA.Data
{
    public class MvcMovieContext:DbContext
    {
        public MvcMovieContext (DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; }
    }
}