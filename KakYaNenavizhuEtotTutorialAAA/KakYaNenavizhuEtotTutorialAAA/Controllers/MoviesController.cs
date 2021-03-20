using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KakYaNenavizhuEtotTutorialAAA.Data;
using KakYaNenavizhuEtotTutorialAAA.Models;

namespace KakYaNenavizhuEtotTutorialAAA.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;

        public MoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string movieGenre, string searchString, string movieRating)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Movie
                orderby m.Genre
                select m.Genre;
            Console.WriteLine("DB Query: " + genreQuery.ToQueryString());
            IQueryable<string> RatingQuery = from m in _context.Movie
                orderby m.Rating
                select m.Rating;
            Console.WriteLine("DB Query: " + genreQuery.ToQueryString()); 
            var movies = from m in _context.Movie                          
                select m;                                                  
            Console.WriteLine("DB Query: " + movies.ToQueryString());
            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
                Console.WriteLine("DB Query: " + movies.ToQueryString());
            }

            if (!string.IsNullOrEmpty(movieGenre) && string.IsNullOrEmpty(movieRating))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
                Console.WriteLine("DB Query: " + movies.ToQueryString());
            }
            
            if (!string.IsNullOrEmpty(movieRating) && string.IsNullOrEmpty(movieGenre))                       
            {                                                            
                movies = movies.Where(x => x.Rating == movieRating);       
                Console.WriteLine("DB Query: " + movies.ToQueryString());
            }
            if (!string.IsNullOrEmpty(movieRating) && !string.IsNullOrEmpty(movieGenre)) 
            {                                                                           
                movies = movies.Where(x => x.Rating == movieRating).Where(x=>x.Genre == movieGenre);                     
                Console.WriteLine("DB Query: " + movies.ToQueryString());               
            }                                                                           

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Rating = new SelectList(await RatingQuery.Distinct().ToListAsync()),
                Movies = await movies.ToListAsync()
            };
                                                                                                                            

            return View(movieGenreVM);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            Console.WriteLine("DB Query: " + "SELECT DISTINCT ON (Id) * \n FROM [Movie] AS [m] \n WHERE [id] = [Id]");  
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            Console.WriteLine("DB Query: " + "SELECT * \n FROM [Movie] AS [m] \n WHERE [id] = [Id]");      
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            Console.WriteLine("DB Query: " + "SELECT DISTINCT ON (Id) * \n FROM [Movie] AS [m] \n WHERE [id] = [Id]");
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            Console.WriteLine("DB Query: " + "SELECT * \n FROM [Movie] AS [m] \n WHERE [id] = [Id]");    
            _context.Movie.Remove(movie);
            Console.WriteLine("Removing movie. Id = " + movie.Id);
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            Console.WriteLine("Executing MovieExistsMethod"); 
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
