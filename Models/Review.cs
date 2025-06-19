using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.API.Models
{

    public class Review
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;

        public int GameId { get; set; }
        public Game Game { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int Rating { get; internal set; }
    }
}