using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json;

namespace GamingPlatform.Models
{
    public class PetitBacPlayer
    {
        public int Id { get; set; } // Identifiant unique du joueur
        public string Name { get; set; } // Nom du joueur

        // Réponses pour toutes les catégories dans une partie (non mappé directement à la base)
        [NotMapped]
        public Dictionary<string, string> Responses { get; set; } = new Dictionary<string, string>();

        // Propriété pour stocker le dictionnaire en base sous forme de JSON
        public string ResponsesJson
        {
            get => JsonSerializer.Serialize(Responses);
            set => Responses = string.IsNullOrEmpty(value)
                ? new Dictionary<string, string>()
                : JsonSerializer.Deserialize<Dictionary<string, string>>(value);
        }

        // Relation avec PetitBacGame
        public int PetitBacGameId { get; set; } // Clé étrangère vers la partie
        public PetitBacGame PetitBacGame { get; set; } // Référence à la partie associée
    }
}
