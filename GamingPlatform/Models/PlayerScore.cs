namespace GamingPlatform.Models
{
public class PlayerScore
{
    public string PlayerId { get; set; }
    public Player Player{get; set;}
    public int WPM { get; set; }
    public double Accuracy { get; set; }

    public int Points { get; set; }
}


}