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
                Letter = 'A', // Lettre par défaut
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
public IActionResult ConfigureGame(PetitBacGame model, string[] SelectedCategories)
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

    }
    }
    