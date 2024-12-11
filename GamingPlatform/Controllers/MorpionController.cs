using Microsoft.AspNetCore.Mvc;
using GamingPlatform.Services;
using Microsoft.EntityFrameworkCore;
using GamingPlatform.Models;

namespace GamingPlatform.Controllers
{
    public class MorpionController : Controller
    {
        public IActionResult Play(Guid lobbyId)
        {
            ViewBag.LobbyId = lobbyId;
            return View("Morpion");
        }
    }
}
