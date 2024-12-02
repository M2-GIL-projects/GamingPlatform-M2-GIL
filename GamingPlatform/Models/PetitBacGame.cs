using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamingPlatform.Models
{
    public class PetitBacGame
    {
        public int Id { get; set; } // Identifiant unique de la partie
        public required List<string> Categories { get; set; } // Catégories choisies
        public required char Letter { get; set; } // Lettre imposée
        public required string EndCondition { get; set; } // Condition d'arrêt : "AllPlayersDone" ou "TimeLimit"
        public int TimeLimit { get; set; } // Temps limite en secondes (si applicable)
        public List<PetitBacPlayer> Players { get; set; } // Liste des joueurs participant
        public bool IsGameStarted { get; set; } // Indique si la partie a commencé

        // Relation avec Lobby
        [ForeignKey("Lobby")]
        public Guid LobbyId { get; set; }
        public Lobby Lobby { get; set; } // Le lobby associé à cette partie
    }

    public class PetitBacPlayer
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public bool HasFinished { get; set; }

        public List<Answer> Answers { get; set; } = new List<Answer>();

        // Relation avec PetitBacGame
        public int PetitBacGameId { get; set; }
        public PetitBacGame PetitBacGame { get; set; }
    }
}

