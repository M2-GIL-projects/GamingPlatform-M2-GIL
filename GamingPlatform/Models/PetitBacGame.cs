using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamingPlatform.Models
{
    public class PetitBacGame
    {
        public int Id { get; set; } // Identifiant unique de la partie
        public char Letter { get; set; } // Lettre choisie pour la partie
        public string EndCondition { get; set; } // Condition d'arrêt
        public int? TimeLimit { get; set; } // Temps limite (optionnel)
        public bool IsGameStarted { get; set; } = false; // Indique si la partie a commencé
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Date de création

       public List<PetitBacCategory> Categories { get; set; } = new List<PetitBacCategory>();

        [NotMapped]
        public List<string> AvailableCategories { get; set; } = new List<string>
        {
            "Pays", "Animaux", "Fruits", "Prénoms", "Villes", "Métiers",
            "Couleurs", "Sports", "Objets", "Plantes", "Marques",
            "Fleurs", "Langues", "Instruments de musique", "Films",
            "Séries", "Livres", "Paysages", "Nourriture", "Boissons"
        };

        // Liste des joueurs
        public List<PetitBacPlayer> Players { get; set; } = new List<PetitBacPlayer>();

        // Relation avec Lobby
        public Guid LobbyId { get; set; }
        public Lobby Lobby { get; set; } // Le lobby associé à cette partie
    }
}
