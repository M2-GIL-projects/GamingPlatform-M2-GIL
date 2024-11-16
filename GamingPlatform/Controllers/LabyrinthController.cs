using GamingPlatform.Hubs;
using GamingPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GamingPlatform.Controllers
{
    
    public class LabyrinthController : Controller
    {
        private readonly LabyrinthGenerator _labyrinthGenerator;

        public LabyrinthController(IHubContext<LabyrinthHub> hubContext)
        {
            _labyrinthGenerator = new LabyrinthGenerator(hubContext);
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Maze()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateMaze(int rows = 10, int cols = 10)
        {
            await _labyrinthGenerator.GenerateLabyrinth(rows, cols);
            return Ok(); // Retourne une réponse OK après avoir généré et envoyé le labyrinthe via SignalR
        }

    }

}
