using GamingPlatform.Models;
using GamingPlatform.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Linq;


namespace GamingPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGameService _gameService;

        public HomeController(ILogger<HomeController> logger, IGameService gameService)
        {
            _logger = logger;
            _gameService = gameService;
        }

        public IActionResult Index()
        {
            var games = _gameService.GetAvailableGames();
            return View(games);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Nouvelle action pour afficher le formulaire d'ajout de jeu
        public IActionResult AddGame()
        {
            var gameTypes = _gameService.GetGameTypes();
            if (gameTypes == null || !gameTypes.Any())
            {
                ViewBag.GameTypes = new List<SelectListItem>();
                ViewBag.NoGameTypes = true;
            }
            else
            {
                ViewBag.GameTypes = new SelectList(gameTypes, "Value", "Text");
                ViewBag.NoGameTypes = false;
            }
            return View();
        }


        [HttpPost]
public IActionResult AddGame(GameCreateModel model)
{
    if (ModelState.IsValid)
    {
        try
        {
            var newGame = _gameService.CreateGame(model);
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
    }
    
    // Si nous arrivons ici, quelque chose s'est mal passé, rechargez la liste des types de jeux
    ViewBag.GameTypes = _gameService.GetGameTypes();
    return View(model);
}


       [HttpPost]
public IActionResult CreateGameType(string name)
{
    try
    {
        _gameService.CreateGameType(name);
        return Json(new { success = true, message = "Type de jeu créé avec succès." });
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = ex.Message });
    }
}



    }
}
