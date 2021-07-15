using System;
using System.Collections.Generic;

namespace ChallengeBackendDisney.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Image{ get; set; }
        public string Name { get; set; }
        public virtual ICollection<Movie> AssociatedMovies { get; set; }
    }
}







