using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.API.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public double Rating { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string CoverUrl { get; set; } = string.Empty;

        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
    }
}