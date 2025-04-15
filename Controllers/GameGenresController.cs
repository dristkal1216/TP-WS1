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
    public class GameGenresController : Controller
    {
        private readonly Tp1Ws1JeuxVideoContext _context;

        public GameGenresController(Tp1Ws1JeuxVideoContext context)
        {
            _context = context;
        }

        // GET: GameGenres
        public async Task<IActionResult> Index(int? id)
        {
            var Gamegenres = _context.GameGenres;

            if (id != null)
            {
                await Gamegenres.Include(g => g.Games)
                      .Where(g => g.GameGenreId == id.ToString())
                      .ToListAsync();
            }
            else
            {
                await Gamegenres.Include(g => g.Games).ToListAsync();
            }


            return View(Gamegenres);
        }

        // GET: GameGenres/Details/5
        public async Task<IActionResult> Details(string id)
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

        // GET: GameGenres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GameGenres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameGenreId,FullName,IsArchived")] GameGenre gameGenre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameGenre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gameGenre);
        }

        // GET: GameGenres/Edit/5
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
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var gameGenre = await _context.GameGenres.FindAsync(id);
            if (gameGenre != null)
            {
                _context.GameGenres.Remove(gameGenre);
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
