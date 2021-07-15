using System;
using System.Collections.Generic;

namespace ChallengeBackendDisney.Models
{
    public class Movie
    {

        public int MovieId { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int Score { get; set; }

        public int GenreId { get; set; }
       public Genre genre { get; set; }

        public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}









