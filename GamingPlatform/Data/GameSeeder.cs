

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
			// Liste des jeux à insérer ou mettre à jour
			var games = new List<Game>
	{
		new Game { 
			Code = "SPT", Name = "SpeedTyping", 
			Description = 
			"Le jeu Speed Typing est un jeu de dactylographie. C’est une activité interactive qui consiste à taper les mots qui apparaissent à l’écran, le plus correctement et le plus rapidement possible", ImageUrl = "/images/speedtyping.png" },
		new Game { Code = "MOR", Name = "Morpion", 
		Description = "Le morpion est un jeu de réflexion se pratiquant à deux joueurs au tour par tour et dont le but est de créer le premier un alignement sur une grille. Le jeu se joue généralement avec papier et crayon.", ImageUrl = "/images/morpion.png" },
		//new Game { Code = "BTN", Name = "BatailleNavale", Description = "Un jeu de stratégie navale.", ImageUrl = "/images/bataillenavale.png" },
		new Game { Code = "PTB", Name = "PetitBac", 
		Description = "Le Petit Bac repose sur la rapidité et la connaissance générale des joueurs. L’objectif est de remplir des catégories prédéterminées avec des mots commençant par une lettre choisie. Les joueurs doivent trouver le maximum de réponses correctes.", ImageUrl = "/images/petitbac.png" },
		new Game
		{
			Code = "LAB",
			Name = "Course de Labyrinthe",
			Description = "Principe du jeu\r\n" +
						  "Dans ce jeu de course de labyrinthe, deux joueurs s'affrontent pour être le premier à atteindre le point de sortie. Le labyrinthe est en constante évolution, ce qui ajoute un élément stratégique et imprévisible à la course. \r\n" +
						  "Matériel\r\n•\tUn plateau de jeu représentant un labyrinthe modifiable\r\n•\t" +
						  "Des tuiles de labyrinthe mobiles\r\n•\tDeux pions (un pour chaque joueur)\r\n•\t" +
						  "Un marqueur pour indiquer le point de sortie\r\nMise en place\r\n1.\t" +
						  "Disposez les tuiles du labyrinthe de manière aléatoire sur le plateau.\r\n2.\t" +
						  "Placez le marqueur de sortie à l'opposé des positions de départ des joueurs.\r\n3.\t" +
						  "Chaque joueur place son pion sur sa case de départ.\r\nDéroulement d'un tour\r\n1.\t" +
						  "Modification du labyrinthe : \r\no\tLe joueur actif insère une tuile de labyrinthe dans une rangée ou colonne du plateau.\r\no\t" +
						  "Cette action modifie les chemins disponibles dans le labyrinthe.\r\n2.\t" +
						  "Déplacement du pion : \r\no\tAprès avoir modifié le labyrinthe, le joueur peut déplacer son pion.\r\no\t" +
						  "Le déplacement se fait en suivant les couloirs ouverts du labyrinthe.\r\no\t" +
						  "Le joueur peut se déplacer aussi loin qu'il le souhaite tant qu'il suit un chemin ininterrompu.\r\n",
			ImageUrl = "/images/labyrinthe.png"
		}
	};

			// Parcourir les jeux pour vérifier leur existence ou les mettre à jour
			foreach (var game in games)
			{
				var existingGame = _context.Game.FirstOrDefault(g => g.Code == game.Code);
				if (existingGame == null)
				{
					// Ajouter un nouveau jeu si non trouvé
					_context.Game.Add(new Game
					{
						Id = Guid.NewGuid(),
						Code = game.Code,
						Name = game.Name,
						Description = game.Description,
						ImageUrl = game.ImageUrl
					});
				}
				else
				{
					// Mettre à jour les propriétés du jeu existant si nécessaire
					existingGame.Name = game.Name;
					existingGame.Description = game.Description;
					existingGame.ImageUrl = game.ImageUrl;
				}
			}

			// Sauvegarder les modifications dans la base de données
			_context.SaveChanges();
		}

	}
}
