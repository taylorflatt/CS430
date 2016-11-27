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
    public class AlbumsController : Controller
    {
        private readonly NotownContext _context;

        public AlbumsController(NotownContext context)
        {
            _context = context;    
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            var notownContext = _context.Album.Include(a => a.Musician);
            return View(await notownContext.ToListAsync());
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .Include(a => a.Musician)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            List<SelectListItem> temp = new List<SelectListItem>();
            foreach(var musician in _context.Musician)
            {
                temp.Add(new SelectListItem
                {
                    Text = musician.Name + " (" + musician.Ssn + ")",
                    Value = Convert.ToString(musician.ID)
                });
            }

            ViewData["MusicianID"] = temp;

            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Speed,CopyrightDate,MusicianID")] Album album)
        {
            if (ModelState.IsValid)
            {
                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["MusicianSsn"] = new SelectList(_context.Musician, "ID", "Ssn", album.MusicianID);
            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album.SingleOrDefaultAsync(m => m.ID == id);
            if (album == null)
            {
                return NotFound();
            }
            ViewData["MusicianID"] = new SelectList(_context.Musician, "ID", "Ssn", album.MusicianID);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Speed,CopyrightDate,MusicianID")] Album album)
        {
            if (id != album.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.ID))
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
            ViewData["MusicianSsn"] = new SelectList(_context.Musician, "ID", "Ssn", album.MusicianID);
            return View(album);
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .Include(a => a.Musician)
                .Include(s => s.Songs)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var album = await _context.Album
                .Include(s => s.Songs)
                .SingleOrDefaultAsync(m => m.ID == id);

            var songList = new List<Song>();

            // Remove all the songs associated with the album.
            foreach (var song in album.Songs)
            {
                _context.Song.Remove(song);
            }

            _context.Album.Remove(album);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AlbumExists(int id)
        {
            return _context.Album.Any(e => e.ID == id);
        }
    }
}
