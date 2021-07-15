namespace ChallengeBackendDisney.DTOs
{
    public class  CharacterRequestDto
    {
        public int CharacterId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public string History { get; set; }
        public int? MovieId { get; set; }
    }
}