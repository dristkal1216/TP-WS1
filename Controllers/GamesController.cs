using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP_WS1.Models;

namespace TP_WS1.Controllers
{
    public class GamesController : Controller
    {
        private readonly Tp1Ws1JeuxVideoContext _context;

        public GamesController(Tp1Ws1JeuxVideoContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index(string? id)
        {
            if (id != null)
            {
                var tp1Ws1JeuxVideoContext = _context.Games.Include(g => g.GameGenre).Include(g => g.User).Where(g => g.GameGenreId == id && g.IsArchived == false);
                return View(await tp1Ws1JeuxVideoContext.ToListAsync());
            } else
            {
                var tp1Ws1JeuxVideoContext = _context.Games.Include(g => g.GameGenre).Include(g => g.User).Where(g => g.IsArchived == false);
                return View(await tp1Ws1JeuxVideoContext.ToListAsync());
            };
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.GameGenre)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            ViewData["GameGenreId"] = new SelectList(_context.GameGenres, "GameGenreId", "GameGenreId");
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameId,Name,GameGenreId,IsOnline,GameEngine,IsArchived")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameGenreId"] = new SelectList(_context.GameGenres, "GameGenreId", "GameGenreId", game.GameGenreId);
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", game.UserId);
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["GameGenreId"] = new SelectList(_context.GameGenres, "GameGenreId", "GameGenreId", game.GameGenreId);
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", game.UserId);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,Name,GameGenreId,IsOnline,GameEngine,UserId,IsArchived")] Game game)
        {
            if (id != game.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
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
            ViewData["GameGenreId"] = new SelectList(_context.GameGenres, "GameGenreId", "GameGenreId", game.GameGenreId);
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", game.UserId);
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.GameGenre)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }
    }
}
