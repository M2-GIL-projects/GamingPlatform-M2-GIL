using System.ComponentModel.DataAnnotations.Schema;
using GamingPlatform.Models;

namespace GamingPlatform.Models
{
public class SpeedTypingGame : Game
{
    public string TextToType { get; set; }

        [NotMapped]
        public Dictionary<Joueur, string> PlayerProgress { get; set; } = new Dictionary<Joueur, string>();

        public override void Start()
        {
            TextToType = GenerateRandomText();
            PlayerProgress = Players.ToDictionary(p => p, _ => "");
            StartTime = DateTime.Now;
            Status = GameStatus.InProgress;
        }

        public override void End()
        {
            Status = GameStatus.Finished;
        }



    private string GenerateRandomText()
    {
        string[] sentences = new string[]
        {
        "The quick brown fox jumps over the lazy dog.",
        "A journey of a thousand miles begins with a single step.",
        "To be or not to be, that is the question.",
        "All that glitters is not gold.",
        "Where there's a will, there's a way.",
        "Actions speak louder than words.",
        "Knowledge is power, guard it well.",
        "Practice makes perfect.",
        "Two wrongs don't make a right.",
        "When in Rome, do as the Romans do."
        };

        Random random = new Random();
        int sentenceCount = random.Next(3, 6); // Choisir entre 3 et 5 phrases

        List<string> selectedSentences = new List<string>();
        for (int i = 0; i < sentenceCount; i++)
        {
            int index = random.Next(0, sentences.Length);
            selectedSentences.Add(sentences[index]);
        }

        return string.Join(" ", selectedSentences);
    }

}
}