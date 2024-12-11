using GamingPlatform.Models;
using System;

namespace GamingPlatform.Models
{
public class Score
{
    public Guid Id { get; set; }
    public Guid LobbyId { get; set; }
    public string PlayerPseudo { get; set; }
    public int WPM { get; set; }
    public double Accuracy { get; set; }
    public int RawScore { get; set; }
    public int AdjustedScore { get; set; }
    public Difficulty Difficulty { get; set; }
    public DateTime Timestamp { get; set; }

}
}