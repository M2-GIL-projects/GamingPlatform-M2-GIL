using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamingPlatform.Models
{
    public abstract class Game
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public GameStatus Status { get; set; }

        // Foreign key for GameType
        public int GameTypeId { get; set; }

        // Navigation property to GameType
        public GameType GameType { get; set; }

        [NotMapped]
        public List<Joueur> Players { get; set; } = new List<Joueur>();

        public DateTime StartTime { get; set; }

        public abstract void Start();
        public abstract void End();

        // Navigation property for Lobby
        public Guid? LobbyId { get; set; }
        public Lobby Lobby { get; set; }
    }

    public enum GameStatus
    {
        NotStarted,
        InProgress,
        Finished
    }
}
