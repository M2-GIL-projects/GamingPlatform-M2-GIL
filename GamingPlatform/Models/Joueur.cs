using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
namespace GamingPlatform.Models
{
public class Joueur
{
    public int Id { get; set; }    
    public string Pseudo { get; set; }
    public List<PlayerLobby> PlayerLobbies { get; set; } = new List<PlayerLobby>();
    public List<Score> Scores { get; set; } = new List<Score>();
}
}

