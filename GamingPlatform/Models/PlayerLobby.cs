//table de jonction
namespace GamingPlatform.Models
{
public class PlayerLobby
{
    public int JoueurId { get; set; }
    public Joueur Joueur { get; set; }

    public Guid LobbyId { get; set; }
    public Lobby Lobby { get; set; }
}
}