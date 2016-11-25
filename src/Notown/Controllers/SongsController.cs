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
    public class SongsController : Controller
    {
        private readonly NotownContext _context;

        public SongsController(NotownContext context)
        {
            _context = context;    
        }

        // GET: Songs
        public async Task<IActionResult> Index()
        {
            var notownContext = _context.Song.Include(s => s.Album).Include(s => s.Musician);
            return View(await notownContext.ToListAsync());
        }

        // GET: Songs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Song
                .Include(s => s.Album)
                .Include(s => s.Musician)
                .SingleOrDefaultAsync(m => m.SongID == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // GET: Songs/Create
        public IActionResult Create()
        {
            ViewData["AlbumID"] = new SelectList(_context.Album, "ID", "ID");
            ViewData["MusicianSsn"] = new SelectList(_context.Musician, "Ssn", "Ssn");
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SongID,Title,MusicianSsn,AlbumID")] Song song)
        {
            if (ModelState.IsValid)
            {
                _context.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["AlbumID"] = new SelectList(_context.Album, "ID", "ID", song.AlbumID);
            ViewData["MusicianSsn"] = new SelectList(_context.Musician, "Ssn", "Ssn", song.MusicianSsn);
            return View(song);
        }

        // GET: Songs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Song.SingleOrDefaultAsync(m => m.SongID == id);
            if (song == null)
            {
                return NotFound();
            }
            ViewData["AlbumID"] = new SelectList(_context.Album, "ID", "ID", song.AlbumID);
            ViewData["MusicianSsn"] = new SelectList(_context.Musician, "Ssn", "Ssn", song.MusicianSsn);
            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SongID,Title,MusicianSsn,AlbumID")] Song song)
        {
            if (id != song.SongID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(song);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongExists(song.SongID))
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
            ViewData["AlbumID"] = new SelectList(_context.Album, "ID", "ID", song.AlbumID);
            ViewData["MusicianSsn"] = new SelectList(_context.Musician, "Ssn", "Ssn", song.MusicianSsn);
            return View(song);
        }

        // GET: Songs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Song
                .Include(s => s.Album)
                .Include(s => s.Musician)
                .SingleOrDefaultAsync(m => m.SongID == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var song = await _context.Song.SingleOrDefaultAsync(m => m.SongID == id);
            _context.Song.Remove(song);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SongExists(int id)
        {
            return _context.Song.Any(e => e.SongID == id);
        }
    }
}
