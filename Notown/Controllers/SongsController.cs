using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Notown.Data;
using Notown.Models;
using Notown.Models.NotownViewModels;

namespace Notown.Controllers
{
    public class SongsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SongsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Collection
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Songs.Include(s => s.Album);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Collection/Details/5
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

        // GET: Collection/Create
        public IActionResult Create()
        {
            ViewData["albumId"] = new SelectList(_context.Album, "albumIdentifier", "albumIdentifier");
            return View();
        }

        // POST: Collection/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("songId,albumId,name,title")] Songs songs)
        {
            if (ModelState.IsValid)
            {
                //var musicianId = _context.Musicians.(songs.albumId);
                songs.Musicians.id = 1;

                _context.Add(songs);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["albumId"] = new SelectList(_context.Album, "albumIdentifier", "albumIdentifier", songs.albumId);
            return View(songs);
        }

        // GET: Collection/Edit/5
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
            ViewData["albumId"] = new SelectList(_context.Album, "albumIdentifier", "albumIdentifier", songs.albumId);
            return View(songs);
        }

        // POST: Collection/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("songId,albumId,author,title")] Songs songs)
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
            ViewData["albumId"] = new SelectList(_context.Album, "albumIdentifier", "albumIdentifier", songs.albumId);
            return View(songs);
        }

        // GET: Collection/Delete/5
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

        // POST: Collection/Delete/5
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
