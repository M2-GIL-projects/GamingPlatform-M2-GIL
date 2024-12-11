using GamingPlatform.Hubs;
using GamingPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

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
        public async Task<IActionResult> Maze(int rows = 10, int cols = 10)
        {
            // Génération du labyrinthe
            bool[,] adjacencyMatrix = await _labyrinthGenerator.GenerateLabyrinth(rows, cols);

            // Sérialisation de la matrice d'adjacence en JSON pour l'utiliser dans la vue
            var adjacencyJson = JsonConvert.SerializeObject(adjacencyMatrix);

            // Transmettre le JSON à la vue
            ViewBag.AdjacencyMatrix = adjacencyJson;

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
