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

}
}