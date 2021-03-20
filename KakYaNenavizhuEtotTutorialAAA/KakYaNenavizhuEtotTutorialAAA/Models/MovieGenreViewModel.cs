using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KakYaNenavizhuEtotTutorialAAA.Models
{
    public class MovieGenreViewModel
    {
        public List<Movie> Movies { get; set; }
        public SelectList Genres { get; set; }
        public SelectList Rating { get; set; }
        public string MovieGenre { get; set; }
        public string MovieRating { get; set; }
        public string SearchString { get; set; }
    }
}