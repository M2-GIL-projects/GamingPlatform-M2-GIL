using Microsoft.AspNetCore.Mvc;
using GamingPlatform.Data;
using GamingPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace GamingPlatform.Controllers
{
    public class PetitBacController : Controller
    {
        private readonly GamingPlatformContext _context;

        public PetitBacController(GamingPlatformContext context)
        {
            _context = context;
        }
[HttpGet]
public IActionResult Configure(Guid lobbyId)
{
    var model = new PetitBacGame
    {
        LobbyId = lobbyId,
        Letter = 'A',         // Lettre par défaut
        //EndCondition = "AllPlayersDone" // Condition d'arrêt par défaut

    };
    return View("Configuration", model); // Retourne la vue Configuration.cshtml
}


[HttpGet]
//tester la connexion avec la base de données 
public IActionResult TestDatabase()
{
    try
    {
        var games = _context.PetitBacGames.Include(g => g.Categories).ToList();
        return Json(games);
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = "Erreur lors de la récupération des données.", error = ex.Message });
    }
}
public IActionResult ConfigureGame(PetitBacGame model, string[] SelectedCategories)
{
    try
    {
        Console.WriteLine($"LobbyId: {model.LobbyId}");
        Console.WriteLine($"Letter: {model.Letter}");
        Console.WriteLine($"EndCondition: {model.EndCondition}");
        Console.WriteLine($"SelectedCategories: {string.Join(", ", SelectedCategories)}");

        // Charger le lobby
        var lobby = _context.Lobby.FirstOrDefault(l => l.Id == model.LobbyId);
        if (lobby == null)
        {
            Console.WriteLine("Lobby introuvable !");
            return NotFound("Lobby introuvable.");
        }

        model.Lobby = lobby;

        // Ajouter les catégories sélectionnées
        foreach (var category in SelectedCategories)
        {
            model.Categories.Add(new PetitBacCategory { Name = category });
        }

        // Enregistrer dans la base de données
        _context.PetitBacGames.Add(model);
        _context.SaveChanges();

        Console.WriteLine("Partie configurée avec succès !");
        return RedirectToAction("Details", "Lobby", new { id = model.LobbyId });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur : {ex.Message}");
        return BadRequest(new { message = "Erreur lors de la configuration du jeu.", error = ex.Message });
    }
}



[HttpGet]
public IActionResult GetGameConfiguration(int gameId)
{
    // Récupérer le jeu avec ses catégories et ses informations
    var game = _context.PetitBacGames
        .Include(g => g.Categories)
        .FirstOrDefault(g => g.Id == gameId);

    if (game == null)
    {
        return NotFound("Jeu introuvable.");
    }

    // Retourner la configuration à la vue
    return View(game);
}



 [HttpGet]
        public IActionResult test(int gameId, int playerId)
        {
            // Passer les valeurs gameId et playerId à la vue
            ViewBag.GameId = gameId;
            ViewBag.PlayerId = playerId;

            // Renvoyer la vue GetGameConfiguration
            return View("GetGameConfiguration");
        }
    }
}
    