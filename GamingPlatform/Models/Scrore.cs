using GamingPlatform.Models;
using System;

namespace GamingPlatform.Models
{
public class Score
{
    public int Id { get; set; }
    public int Value { get; set; }
    public DateTime Date { get; set; }
    public double TimeTaken { get; set; }

    // Navigation properties
    public int UserId { get; set; }
    public Joueur Joueur { get; set; }

    public Guid GameId { get; set; }
    public Game Game { get; set; }
}
}