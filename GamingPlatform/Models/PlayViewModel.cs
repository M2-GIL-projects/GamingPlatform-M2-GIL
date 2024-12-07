namespace GamingPlatform.Models
{
public class PlayViewModel
{
    public Lobby Lobby { get; set; }
    public List<int> PlayerIds { get; set; } = new List<int>(); // Initialiser pour éviter null
}


}