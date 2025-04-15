using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP_WS1.Models;
using TP_WS1.ViewModels;

namespace TP_WS1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Tp1Ws1JeuxVideoContext _context;

        public HomeController(ILogger<HomeController> logger, Tp1Ws1JeuxVideoContext context)
        {
            _context = context;
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

           

            var results = _context.GameGenres.Select(g => new GGenreGame
            {
                fullName = g.GameGenreId,
                Top3Games = _context.Games.Where(game => game.GameGenreId == g.GameGenreId)
                             .OrderByDescending(game => game.UpdatedAt)
                             .Take(3)
                             .ToList()
            });

            return View(results);
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
    }
}
