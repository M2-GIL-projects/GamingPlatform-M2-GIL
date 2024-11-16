using GamingPlatform.Services;
using Microsoft.AspNetCore.Mvc;

namespace GamingPlatform.Controllers{
public class GameController : Controller
{
    
    private readonly IGameService _gameService;
    public IActionResult SpeedTyping()
    {
        return View();
    }

    public IActionResult Morpion()
    {
        return View();
    }

    public IActionResult BatailleNavale()
    {
        return View();
    }

    public IActionResult PetitBac()
    {
        return View();
    }

    public IActionResult CourseLabyrinthe()
    {
        return View();
    }
}

}
