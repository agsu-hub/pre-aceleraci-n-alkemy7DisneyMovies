using System.Collections.Generic;

namespace ChallengeBackendDisney.Models
{
    public class Character
    {

        public int CharacterId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public string History { get; set; }



        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
