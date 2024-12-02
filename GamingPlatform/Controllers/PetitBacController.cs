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
        EndCondition = "AllPlayersDone" // Condition d'arrêt par défaut

    };
    return View("Configuration", model); // Retourne la vue Configuration.cshtml
}


     [HttpPost]
public IActionResult ConfigureGame(PetitBacGame model, string[] SelectedCategories)
{
    if (ModelState.IsValid)
    {
        // Ajouter les catégories sélectionnées
        model.SelectedCategories = SelectedCategories.ToList();

        // Enregistrer dans la base de données
        _context.PetitBacGames.Add(model);
        _context.SaveChanges();

        // Rediriger vers les détails du lobby
        return RedirectToAction("Details", "Lobby", new { id = model.LobbyId });
    }

    return View("Configure", model); // Si invalid, retourner à la vue Configure
}
    }
}
