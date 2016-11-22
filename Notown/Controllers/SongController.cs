using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Notown.Data;
using Notown.Models;

namespace Notown.Controllers
{
    public class SongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SongController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Song
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Songs.Include(s => s.Album);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Song/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songs = await _context.Songs.SingleOrDefaultAsync(m => m.songId == id);
            if (songs == null)
            {
                return NotFound();
            }

            return View(songs);
        }

        // GET: Song/Create
        public IActionResult Create()
        {
            ViewData["albumIdForeignKey"] = new SelectList(_context.Album, "albumID", "title");
            ViewData["musicianIdForeignKey"] = new SelectList(_context.Musicians, "id", "name");
            return View();
        }

        // POST: Song/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("songId,albumIdForeignKey,musicianIdForeignKey,title")] Songs songs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(songs);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["albumIdForeignKey"] = new SelectList(_context.Album, "albumID", "title", songs.albumIdForeignKey);
            ViewData["musicianIdForeignKey"] = new SelectList(_context.Musicians, "id", "name", songs.musicianIdForeignKey);
            return View(songs);
        }

        // GET: Song/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songs = await _context.Songs.SingleOrDefaultAsync(m => m.songId == id);
            if (songs == null)
            {
                return NotFound();
            }
            ViewData["albumIdForeignKey"] = new SelectList(_context.Album, "albumID", "title", songs.albumIdForeignKey);
            return View(songs);
        }

        // POST: Song/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("songId,albumIdForeignKey,title")] Songs songs)
        {
            if (id != songs.songId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(songs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongsExists(songs.songId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["albumIdForeignKey"] = new SelectList(_context.Album, "albumID", "title", songs.albumIdForeignKey);
            return View(songs);
        }

        // GET: Song/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songs = await _context.Songs.SingleOrDefaultAsync(m => m.songId == id);
            if (songs == null)
            {
                return NotFound();
            }

            return View(songs);
        }

        // POST: Song/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var songs = await _context.Songs.SingleOrDefaultAsync(m => m.songId == id);
            _context.Songs.Remove(songs);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SongsExists(int id)
        {
            return _context.Songs.Any(e => e.songId == id);
        }
    }
}
