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
// Afficher le formulaire de configuration
        public IActionResult Configure(Guid lobbyId)
        {
            // Récupérer le lobby depuis le contexte
            var lobby = _context.Lobby.FirstOrDefault(l => l.Id == lobbyId);
            if (lobby == null)
            {
                return NotFound("Lobby introuvable.");
            }

            // Créer une nouvelle instance du jeu si nécessaire
            var model = new PetitBacGame
            {
                LobbyId = lobbyId,
                Letters = new List<char> { 'A' }, 
                PlayerCount = 2, // Par défaut, 2 joueurs
                CreatorPseudo = "", // Par défaut, pseudo vide
                Categories = _context.PetitBacCategories.ToList(), // Charger les catégories disponibles
            };

            // Générer le lien pour inviter des joueurs
            string linkPlayer2 = $"{Request.Scheme}://{Request.Host}/PetitBac/Join?code={lobby.Code}&playerId=2";
            ViewBag.LinkPlayer2 = linkPlayer2;

            return View("Configuration", model);
        }

    [HttpPost]
public IActionResult ConfigureGame(PetitBacGame model, string[] SelectedCategories, string[] SelectedLetters)
{
    try
    {
        // Charger le lobby depuis la base de données
        var lobby = _context.Lobby.FirstOrDefault(l => l.Id == model.LobbyId);
        if (lobby == null)
        {
            Console.WriteLine("Lobby introuvable !");
            return NotFound("Lobby introuvable.");
        }

        // Associer le lobby au jeu
        model.Lobby = lobby;
         // Validation des lettres
        if (SelectedLetters == null || SelectedLetters.Length == 0)
        {
            ModelState.AddModelError("Letters", "Vous devez sélectionner au moins une lettre.");
            return View("Configuration", model);
        }

        // Ajouter les lettres sélectionnées
        model.Letters.AddRange(SelectedLetters.Select(l => l[0]));


        // Validation des catégories
        if (SelectedCategories == null || SelectedCategories.Length == 0)
        {
            ModelState.AddModelError("SelectedCategories", "Vous devez sélectionner au moins une catégorie.");
            return View("Configuration", model);
        }

        // Ajouter les catégories sélectionnées
        foreach (var category in SelectedCategories)
        {
            model.Categories.Add(new PetitBacCategory { Name = category });
        }

        // Enregistrer dans la base de données
        _context.PetitBacGames.Add(model);
        _context.SaveChanges();

        Console.WriteLine("Partie configurée avec succès !");

        // Rediriger vers la page de récapitulatif
        return RedirectToAction("Recapitulatif", new { gameId = model.Id });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur : {ex.Message}");
        return BadRequest(new { message = "Erreur lors de la configuration du jeu.", error = ex.Message });
    }
}

[HttpGet]
public IActionResult Recapitulatif(int gameId)
{
    // Charger la partie depuis la base de données
    var game = _context.PetitBacGames
        .Include(g => g.Categories)
        .Include(g => g.Lobby) // Inclure également le Lobby pour accéder au Code
        .FirstOrDefault(g => g.Id == gameId);

    if (game == null)
    {
        return NotFound("Partie introuvable.");
    }

    // Générer le lien pour inviter les joueurs
    string linkPlayer2 = $"{Request.Scheme}://{Request.Host}/PetitBac/Join?code={game.Lobby.Code}";
    ViewBag.LinkPlayer2 = linkPlayer2;

    // Passer l'objet game à la vue
    return View(game);
}
    

[HttpGet]
public IActionResult Join(string code)
{
    // Trouver le lobby correspondant au code
    var lobby = _context.Lobby.FirstOrDefault(l => l.Code == code);

    if (lobby == null)
    {
        return NotFound("Lobby introuvable.");
    }

    // Trouver le jeu correspondant au lobby
    var game = _context.PetitBacGames
        .Include(g => g.Categories)
        .Include(g => g.Players)
        .FirstOrDefault(g => g.LobbyId == lobby.Id);

    if (game == null)
    {
        return NotFound("Partie introuvable.");
    }

    // Vérifier si le nombre maximal de joueurs est atteint
    if (game.Players.Count >= game.PlayerCount)
    {
        TempData["Error"] = "Le nombre maximal de joueurs pour cette partie a été atteint.";
        return RedirectToAction("ErrorPage"); // Rediriger vers une page d'erreur ou afficher un message
    }

    // Passer les détails du jeu à la vue
    ViewBag.GameId = game.Id;
    return View("RecapitulatifJoin", game);
}
[HttpPost]
public IActionResult RegisterPlayer(int gameId, string pseudo)
{
    // Rechercher ou créer une partie
    var game = _context.PetitBacGames.FirstOrDefault(g => g.Id == gameId);
    if (game == null)
    {
        return NotFound("Jeu introuvable.");
    }

    // Créer un joueur avec un token unique
    var player = new PetitBacPlayer
    {
        Pseudo = pseudo,
        PetitBacGameId = gameId,
        SessionToken = Guid.NewGuid().ToString() // Génération du token
    };

    // Ajouter le joueur dans la base
    _context.PetitBacPlayer.Add(player);
    _context.SaveChanges();

    // Construire l'URL avec le sessionToken
    string url = $"/PlayPetitBac/Play?gameId={gameId}&sessionToken={player.SessionToken}";

    // Rediriger le joueur vers la page de jeu
    return Redirect(url);
}
    }}
    