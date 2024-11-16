using System;
using System.Collections.Generic;
namespace GamingPlatform.Models
{
public class Lobby
{
    public Guid Id { get; set; }
    public string Code { get; set; } = Guid.NewGuid().ToString(); // Code unique pour inviter les joueurs
    public string Name { get; set; }
    public string GameType { get; set; }
    public bool IsPrivate { get; set; }
    public string Password { get; set; } // Mot de passe pour les lobbies privés
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public LobbyStatus Status { get; set; }

    // Navigation properties
   public List<PlayerLobby> Players { get; set; } = new List<PlayerLobby>();
}


public enum LobbyStatus
{
    Waiting,
    InProgress,
    Finished
}

}

