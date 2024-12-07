namespace GamingPlatform.Models
{
    public class SimpleViewModel
    {
        public Guid LobbyId { get; set; }
        public List<string> PlayerPseudos { get; set; } = new List<string>(); // Nouvelle propriété pour les pseudos
    }
}
