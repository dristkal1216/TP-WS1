using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    //[Authorize(Roles = "Admin")]
    //[Authorize(Roles = "User")]
    public class GamesController : Controller
    {
        private readonly Tp1Ws1JeuxVideoContext _context;

        public GamesController(Tp1Ws1JeuxVideoContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index(int? id,DateTime? dateTime)
        {
            if (id != null)
            {

                var result =  _context.Posts.Where(p => p.GameId == id).Select(gp => new GamePost
                {
                    GameName = gp.Game.Name,
                    NbPost = gp.Game.Posts.Count(),
                    NbVue = gp.Click ,
                    author = gp.User.UserName,
                    lastUserActivity = gp.User.UserName,
                    PostId = gp.PostId, 
                    CreatedAt = gp.CreatedAt,
                    UpdateAt = gp.UpdatedAt,
                });

                return View(result);
            } else if (dateTime != null)
            {
                var result = _context.Posts.Where(p => p.UpdatedAt == dateTime).Select(gp => new GamePost
                {
                    GameName = gp.Game.Name,
                    NbPost = gp.Game.Posts.Count(),
                    NbVue = gp.Click,
                    author = gp.User.UserName,
                    lastUserActivity = gp.User.UserName,
                    PostId = gp.PostId,
                    CreatedAt = gp.CreatedAt,
                    UpdateAt = gp.UpdatedAt,
                });

                return View(result);
            }
            else
            {
                var result = _context.Posts.Select(gp => new GamePost
                {
                    GameName = gp.Game.Name,
                    NbPost = gp.Game.Posts.Count(),
                    NbVue = gp.Click,
                    author = gp.User.UserName,
                    lastUserActivity = gp.User.UserName,
                    PostId = gp.PostId,
                    CreatedAt = gp.CreatedAt,
                    UpdateAt = gp.UpdatedAt,
                }); ;
                return View(result);
            }
            ;
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

        // GET: GameGenres/Create
        [Authorize]
        public IActionResult Create()
        {
            // Affiche une vue Create.cshtml où l'on ne demande que le FullName
            return View();
        }

        // POST: GameGenres/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName")] GameGenre gameGenre)
        {
            if (ModelState.IsValid)
            {
                // Génère un nouvel ID
                gameGenre.GameGenreId = Guid.NewGuid().ToString();
                // Associe l'utilisateur courant
                gameGenre.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // Définit l'état par défaut
                gameGenre.IsArchived = false;

                _context.GameGenres.Add(gameGenre);
                await _context.SaveChangesAsync();

                // Redirige par exemple vers l'index des GameGenres
                return RedirectToAction(nameof(Index));
            }
            // En cas d'erreur de validation, on réaffiche le formulaire avec les messages
            return View(gameGenre);
        }




        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            var userIdPost = await _context.Posts.Where(p => p.PostId == id).Select(p => p.UserId).FirstOrDefaultAsync();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == userIdPost || User.IsInRole("Admin"))
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

                ViewData["GameGenreId"] =
                    new SelectList(_context.GameGenres, "GameGenreId", "GameGenreId", game.GameGenreId);
                ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", game.UserId);
                return View(game);
            }
            else
            {
                return RedirectToAction(nameof(Index), new { id = id });
            }
        }



        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,Name,GameGenreId,IsOnline,GameEngine,UserId,IsArchived,UpdatedAt,CreatedAt")] Game game)
        {
            var userIdPost = await _context.Posts.Where(p => p.PostId == id).Select(p => p.UserId).FirstOrDefaultAsync();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == userIdPost || User.IsInRole("Admin"))
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

                    return RedirectToAction(nameof(Index), new { id = id });
                }

                ViewData["GameGenreId"] =
                    new SelectList(_context.GameGenres, "GameGenreId", "GameGenreId", game.GameGenreId);
                ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", game.UserId);
                return View(game);
            }
            else
            {
                return RedirectToAction(nameof(Index), new { id = id });
            }


        }

        // GET: Games/Delete/5
        [Authorize]
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

        [Authorize]
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
