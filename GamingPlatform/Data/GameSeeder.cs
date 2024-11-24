

using GamingPlatform.Models;

namespace GamingPlatform.Data
{
    public class GameSeeder
    {
        private readonly GamingPlatformContext _context;

        public GameSeeder(GamingPlatformContext context)
        {
            _context = context;
        }

        public void SeedGames()
        {
            // Vérifier si les jeux existent déjà
            if (_context.Game.Any())
                return;

            // Liste des jeux à insérer
            var games = new List<Game>
            {
                new Game { Id = Guid.NewGuid(), Name = "SpeedTyping", Code = "SPT", Description = "Un jeu de dactylographie rapide." },
                new Game { Id = Guid.NewGuid(), Name = "Morpion", Code = "MOR", Description = "Le classique jeu du morpion." },
                new Game { Id = Guid.NewGuid(), Name = "BatailleNavale", Code = "BTN", Description = "Un jeu de stratégie navale." },
                new Game { Id = Guid.NewGuid(), Name = "PetitBac", Code = "PTB", Description = "Un jeu d'association de mots par catégories." },
                new Game { Id = Guid.NewGuid(), Name = "Course de Labyrinthe", Code = "LAB", Description = "Un jeu de course dans un labyrinthe." }
            };

            // Ajouter les jeux à la base de données
            _context.Game.AddRange(games);
            _context.SaveChanges();
        }
    }
}
