using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GamingPlatform.Data;
using GamingPlatform.Models;
using System.Collections.Generic;
using System.Linq;

namespace GamingPlatform.Controllers
{
    public class PlayPetitBacController : Controller
    {
        private readonly GamingPlatformContext _context;

        public PlayPetitBacController(GamingPlatformContext context)
        {
            _context = context;
        }
//redirger les joueurs vers une page de jeu
[HttpGet]
public IActionResult Play(int gameId, string sessionToken)
{
    // Charger le jeu
    var game = _context.PetitBacGames
        .Include(g => g.Categories)
        .FirstOrDefault(g => g.Id == gameId);

    if (game == null)
        return NotFound("Jeu introuvable.");

    // Rechercher le joueur avec le token de session
    var player = _context.PetitBacPlayer
        .FirstOrDefault(p => p.SessionToken == sessionToken && p.PetitBacGameId == gameId);

    if (player == null)
        return NotFound("Session de joueur introuvable ou invalide.");

    // Préparer les données pour la vue
    var viewModel = new PlayViewModel
    {
        GameId = gameId,
        PlayerId = player.Id,
        Letters = game.Letters,
        Categories = game.Categories.Select(c => c.Name).ToList()
    };

    return View(viewModel);
}
[HttpPost]
public IActionResult SubmitAnswers(int gameId, int playerId, Dictionary<char, Dictionary<string, string>> answers)
{
    var player = _context.PetitBacPlayer
        .FirstOrDefault(p => p.Id == playerId && p.PetitBacGameId == gameId);

    if (player == null)
        return NotFound("Session de joueur introuvable ou invalide.");

    try
    {
        // Enregistrer les réponses
        player.Responses = answers;
        _context.SaveChanges();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de l'enregistrement des réponses : {ex.Message}");
        return BadRequest("Une erreur est survenue lors de la soumission des réponses.");
    }

    // Redirection vers l'action Play avec le SessionToken
    return RedirectToAction("Play", new { gameId = gameId, sessionToken = player.SessionToken });
}
    }
}