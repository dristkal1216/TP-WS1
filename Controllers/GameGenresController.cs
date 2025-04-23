using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP_WS1.Models;
using TP_WS1.ViewModels;

namespace TP_WS1.Controllers
{
    public class GameGenresController : Controller
    {
        private readonly Tp1Ws1JeuxVideoContext _context;

        public GameGenresController(Tp1Ws1JeuxVideoContext context)
        {
            _context = context;
        }


        
        // GET: GameGenres
        public async Task<IActionResult> Index(int? id = null)
        {
            if (id == null)
            {
                var all = await _context.GameGenres
                    .Select(g => new GGenreGame
                    {
                        fullName = g.FullName,
                        GameGenreId = g.GameGenreId,
                        Top3Games = _context.Games
                            .Where(x => x.GameGenreId == g.GameGenreId)
                            .ToList()           // OK de ToList() synchronously ici
                    })
                    .ToListAsync(); // exécution async de l'ensemble

                return View(all);
            }

            // On filtre, on projette et on exécute la requête en une seule fois
            var vm = await _context.Games
                .Where(g => g.GameId == id.Value)             // == et .Value pour int?
                .Select(g => new GGenreGame
                {
                    fullName = g.GameGenre.FullName,
                    GameGenreId = g.GameGenreId,
                    Top3Games = _context.Games
                        .Where(x => x.GameGenreId == g.GameGenreId) 
                        .ToList()           // OK de ToList() synchronously ici
                })
                .ToListAsync();           // exécution async de l'ensemble

            return View(vm);
        }

       

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Authorize]
        public IActionResult Create(string? genreId)
        {
            // genreId vient par ex. de /Games/Create?genreId=XYZ
            var game = new Game
            {
                GameGenreId = genreId,
                IsArchived = false,
                IsOnline = false
            };
            return View(game);
        }

        // POST: GameGenres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameGenreId,Name,GameEngine,IsOnline,IsArchived")] Game game)
        {
            game.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            game.CreatedAt = DateTime.UtcNow;
            game.UpdatedAt = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Games");
            }
            return View(game);
        }

        // GET: GameGenres/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameGenre = await _context.GameGenres.FindAsync(id);
            if (gameGenre == null)
            {
                return NotFound();
            }
            return View(gameGenre);
        }

        // POST: GameGenres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, [Bind("GameGenreId,FullName,IsArchived")] GameGenre gameGenre)
        {
            if (id != gameGenre.GameGenreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameGenre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameGenreExists(gameGenre.GameGenreId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gameGenre);
        }

        // GET: GameGenres/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameGenre = await _context.GameGenres
                .FirstOrDefaultAsync(m => m.GameGenreId == id);
            if (gameGenre == null)
            {
                return NotFound();
            }

            return View(gameGenre);
        }

        // POST: GameGenres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var gameGenre = await _context.GameGenres.FindAsync(id);
            if (gameGenre != null)
            {
                // Instead of removing:
                // _context.GameGenres.Remove(gameGenre);

                // Soft-delete:
                gameGenre.IsArchived = true;
                _context.GameGenres.Update(gameGenre);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameGenreExists(string id)
        {
            return _context.GameGenres.Any(e => e.GameGenreId == id);
        }
    }
}
