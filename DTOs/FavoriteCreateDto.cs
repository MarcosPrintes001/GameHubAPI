using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GameHub.API.Dtos
{
    public class FavoriteCreateDto
    {
        [Required]
        public int GameId { get; set; }
    }
}
