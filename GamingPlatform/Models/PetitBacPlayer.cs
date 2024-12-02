using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamingPlatform.Models
{
    public class PetitBacPlayer
    {
        public int Id { get; set; } // Identifiant unique du joueur

        public required string PlayerName { get; set; } // Nom du joueur

        public bool HasFinished { get; set; } = false; // Indique si le joueur a terminé sa partie

        public List<Answer> Answers { get; set; } = new List<Answer>(); // Liste des réponses fournies par le joueur

        // Relation avec PetitBacGame
        public int PetitBacGameId { get; set; }
        public PetitBacGame PetitBacGame { get; set; }
    }
}