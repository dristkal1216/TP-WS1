﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP_WS1.Models;

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
        public async Task<IActionResult> Index(int? id, DateTime? dateTime)
        {
            if (id != null)
            {
                var tp1Ws1JeuxVideoContext = _context.Posts.Include(p => p.Game).Include(p => p.User).Where(p => p.GameId == id);
                return View(await tp1Ws1JeuxVideoContext.ToListAsync());
            } else if(dateTime != null)
            {
                var tp1Ws1JeuxVideoContext = _context.Posts.Include(p => p.Game).Include(p => p.User).Where(p => p.UpdatedAt == dateTime);
                return View(await tp1Ws1JeuxVideoContext.ToListAsync());
            }
            else
            {
                var tp1Ws1JeuxVideoContext = _context.Posts.Include(p => p.Game).Include(p => p.User);
                return View(await tp1Ws1JeuxVideoContext.ToListAsync());
            }


        }





        [Authorize]
        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "UserId", "UserId");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,Message,UserId,GameId,ReactionId,IsArchived")] Post post)
        {

            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", post.GameId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "UserId", "UserId", post.UserId);
            return View(post);
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
