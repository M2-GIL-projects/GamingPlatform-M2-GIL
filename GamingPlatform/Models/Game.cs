namespace GamingPlatform.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Code { get; set; } // Utilis� pour reconna�tre le jeu � lancer
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
    }

}
