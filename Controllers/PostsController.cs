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
    public class PostsController : Controller
    {
        private readonly Tp1Ws1JeuxVideoContext _context;

        public PostsController(Tp1Ws1JeuxVideoContext context)
        {
            _context = context;
        }

        // GET: Posts
        public Task<IActionResult> Index(int? id, DateTime? dateTime)
        {
            IQueryable<Post> query = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Game);


            if (id != null)
            {
                query = query.Where(p => p.GameId == id);
            }
            // sinon on laisse query = tous les posts

            var vm = new ViewPost
            {
                ViewPosts = query
                    .Select(p => new Post
                    {
                        PostId = p.PostId,
                        Message = p.Message,
                        CreatedAt = p.CreatedAt,
                        UserId = p.UserId,
                        User = p.User,
                        GameId = p.GameId,
                        Game = p.Game// ← on assigne la nav. property
                    })
                    .ToList(),

                GameId = id ?? throw new ArgumentNullException(nameof(id))
                
            };
            ViewData["HighlightDate"] = dateTime;
            return Task.FromResult<IActionResult>(View("Index", vm));
        }


        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(
            [Bind("GameId,Message,IsArchived")] ViewPost post)
        {
            // on met l’utilisateur et les dates
            post.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            post.CreatedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;

            if (!ModelState.IsValid)
            {
                // re-render avec erreurs
                return View(post);
            }

            // création de l’entité et liaison à la clé étrangère GameId
            var entity = new Post
            {
                GameId = post.GameId,
                Message = post.Message,
                UserId = post.UserId,
                IsArchived = post.IsArchived,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();

            // redirige vers la liste des posts du jeu
            return RedirectToAction(nameof(Index), "Posts",
                new { id = post.GameId });
        }


        // POST: /Posts/Update
        // Mis à jour d’un post existant (inline edit)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            [Bind("PostId,Message")] ViewPost post)
        {

            

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Récupération de l’entité existante
            var entity = await _context.Posts.FindAsync(post.PostId);
            if (entity == null)
                return NotFound();

            // Sécurité : on vérifie l’auteur ou le rôle Admin
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (entity.UserId != currentUserId && !User.IsInRole("Admin"))
                return Forbid();

            // Mise à jour du message et de la date
            entity.Message = post.Message;
            entity.UpdatedAt = DateTime.UtcNow;

            _context.Update(entity);
            await _context.SaveChangesAsync();

            // Retourne vers la liste des posts pour ce jeu
            return RedirectToAction("Index", "Posts",
                new { id = entity.GameId });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();


            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

  
            if (currentUserId == post.UserId || User.IsInRole("Admin"))
            {
                ViewData["GameId"] = new SelectList(
                    _context.Games,
                    "GameId",
                    "Name",     
                    post.GameId
                );
                ViewData["UserId"] = new SelectList(
                    _context.AspNetUsers,
                    "Id",      
                    "UserName",  
                    post.UserId
                );
                return View(post);
            }
            else
            {
                // redirige vers l’Index du même contrôleur en passant l’id
                return RedirectToAction(nameof(Index), new { id });
            }
        }


        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Message,UserId,GameId,ReactionId,IsArchived")] Post post)
        {
            var userIdPost = await _context.Posts.Where(p => p.PostId == id).Select(p => p.UserId).FirstOrDefaultAsync();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == userIdPost || User.IsInRole("Admin"))
            {
                if (id != post.PostId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(post);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PostExists(post.PostId))
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

                ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", post.GameId);
                ViewData["UserId"] = new SelectList(_context.AspNetUsers, "UserId", "UserId", post.UserId);
                return View(post);
            }
            else
            {
                return RedirectToAction(nameof(Index), new { id = id });
            }
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userIdPost = await _context.Posts.Where(p => p.PostId == id).Select(p => p.UserId).FirstOrDefaultAsync();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == userIdPost || User.IsInRole("Admin"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var post = await _context.Posts
                    .Include(p => p.Game)
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(m => m.PostId == id);
                if (post == null)
                {
                    return NotFound();
                }

                return View(post);
            }
            else
            {
                return RedirectToAction(nameof(Index), new { id = id });
            }
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userIdPost = await _context.Posts.Where(p => p.PostId == id).Select(p => p.UserId).FirstOrDefaultAsync();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == userIdPost || User.IsInRole("Admin"))
            {
                var post = await _context.Posts.FindAsync(id);
                if (post != null)
                {
                    _context.Posts.Remove(post);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index), new { id = id });
            }
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
