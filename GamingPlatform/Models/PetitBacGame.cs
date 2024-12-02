using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamingPlatform.Models
{
    public class PetitBacGame
    {
        public int Id { get; set; }

        public char Letter { get; set; }

        public string EndCondition { get; set; }

        public int? TimeLimit { get; set; }

        public bool IsGameStarted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public List<string> AvailableCategories { get; set; } = new List<string>
        {
        
        "Pays", 
        "Animaux",
        "Fruits",
        "Prénoms",
        "Villes",
        "Métiers",
        "Couleurs",
        "Sports",
        "Objets",
        "Plantes",
        "Marques",
        "Fleurs",
        "Langues",
        "Instruments de musique",
        "Films",
        "Séries",
        "Livres",
        "Paysages",
        "Nourriture",
        "Boissons"
        };

        public List<PetitBacPlayer> Players { get; set; } = new List<PetitBacPlayer>();
        public List<string> SelectedCategories { get; set; } = new List<string>();

        // Relation avec Lobby
        [ForeignKey("Lobby")]
        public Guid LobbyId { get; set; }
        public Lobby Lobby { get; set; } // Le lobby associé à cette partie
    }

}


