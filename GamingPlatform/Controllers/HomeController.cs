using GamingPlatform.Models;
using GamingPlatform.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;


namespace GamingPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;

        public HomeController(ILogger<HomeController> logger, GameService gameService, PlayerService playerService)
        {
            _logger = logger;
            _gameService = gameService;
            _playerService = playerService;
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
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendContactForm(string name, string email, string subject, string message)
        {
            // Traitement du formulaire, par exemple envoi d'un email
            // Si l'envoi est r�ussi, on redirige vers la m�me page avec un message de confirmation
            ViewData["MessageSent"] = true;
            return View();

        }

        // Action pour l'enregistrement
        [HttpPost]
        public async Task<IActionResult> RegisterPlayer(string pseudo, string name, string email, string returnUrl = "/")
        {
            var player = await _playerService.AddPlayerAsync(pseudo, name, email);
            if (player == null)
            {
                // Si le pseudo est d�j� pris
                TempData["ErrorMessage"] = "Le pseudo est d�j� pris.";
                return RedirectToAction("Index"); // Ou rediriger vers la page principale
            }

            // Sauvegarder l'id du joueur dans la session pour le rendre accessible dans toute l'application
            HttpContext.Session.SetInt32("PlayerId", player.Id);
            HttpContext.Session.SetString("PlayerPseudo", player.Pseudo);

            return Redirect(returnUrl); // Redirection vers l'URL de retour ou la page d'accueil par d�faut
        }

        // Action pour la connexion
        [HttpPost]
        public async Task<IActionResult> LoginPlayer(string pseudo, string returnUrl = "/")
        {
            var player = await _playerService.GetPlayerByPseudoAsync(pseudo);
            if (player == null)
            {
                TempData["ErrorMessage"] = "Pseudo non trouv�.";
                return RedirectToAction("Index"); // Ou rediriger vers la page principale
            }

            // Sauvegarder l'id du joueur dans la session pour le rendre accessible dans toute l'application
            HttpContext.Session.SetInt32("PlayerId", player.Id);
            HttpContext.Session.SetString("PlayerPseudo", player.Pseudo);

            return Redirect(returnUrl); // Redirection vers l'URL de retour ou la page d'accueil par d�faut
        }

        public async Task<Player> GetCurrentPlayer()
        {
            // R�cup�rer l'ID du joueur depuis la session
            var playerId = HttpContext.Session.GetInt32("PlayerId");

            if (playerId.HasValue)
            {
                return await _playerService.GetPlayerByIdAsync(playerId.Value); 
            }

            return null; 
        }

        // Action pour se d�connecter
        public IActionResult Logout()
        {
            // Supprimer les informations du joueur de la session
            HttpContext.Session.Remove("PlayerId");
            HttpContext.Session.Remove("PlayerPseudo");

            // Rediriger vers la page d'accueil
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Player(string returnUrl = "/")
        {
            var player = await GetCurrentPlayer(); // R�cup�rer le joueur actuel si connect�
            if (player != null)
            {
                // Si le joueur est connect�, on passe l'objet player � la vue
                return View(player);
            }
            else
            {
                // Si aucun joueur n'est connect�, on passe null ou un message appropri�
                ViewBag.returnUrl = returnUrl;
                return View();
            }
        }
        public IActionResult Error()
        {
            return View(); // Redirige vers une vue d'erreur g�n�rique
        }

    }
}
