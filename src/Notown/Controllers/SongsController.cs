using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Notown.Data;
using Notown.Models;
using Notown.Helpers;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageSize = 5;

            ViewData["CurrentSort"] = sortOrder;    // Allows us to keep sort order in paging links.
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["AlbumSortParm"] = sortOrder == "Album" ? "album_desc" : "Album";
            ViewData["MusicianSortParm"] = sortOrder == "Musician" ? "musician_desc" : "Musician";

            // Need to reset paging data because there is new information to display.
            if (searchString != null)
                page = 1;

            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;   // Allows us to keep filters in paging links.

            var songs = from a in _context.Song.Include(m => m.Musician).Include(a => a.Album)
                         select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                songs = songs.Where(a => a.Title.Contains(searchString) ||
                    a.Musician.Name.Contains(searchString) || a.Album.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    songs = songs.OrderByDescending(a => a.Title);
                    break;
                case "Album":
                    songs = songs.OrderBy(a => a.Album.Name);
                    break;
                case "album_desc":
                    songs = songs.OrderByDescending(a => a.Album.Name);
                    break;
                case "Musician":
                    songs = songs.OrderBy(a => a.Musician.Name);
                    break;
                case "musician_desc":
                    songs = songs.OrderByDescending(a => a.Musician.Name);
                    break;
                default:
                    songs = songs.OrderBy(a => a.Title);
                    break;
            }

            return View(await PaginatedList<Song>.CreateAsync(songs.AsNoTracking(), page ?? 1, pageSize));
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
        [Authorize]
        public IActionResult Create()
        {
            ViewData["AlbumID"] = new SelectList(_context.Album, "ID", "Name");
            ViewData["MusicianID"] = new SelectList(_context.Musician, "ID", "Name");
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SongID,Title,MusicianID,AlbumID")] Song song)
        {
            if (ModelState.IsValid)
            {
                _context.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["AlbumID"] = new SelectList(_context.Album, "ID", "Name", song.AlbumID);
            ViewData["MusicianID"] = new SelectList(_context.Musician, "ID", "Name", song.MusicianID);
            return View(song);
        }

        // GET: Songs/Edit/5
        [Authorize]
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
            ViewData["AlbumID"] = new SelectList(_context.Album, "ID", "Name", song.AlbumID);
            ViewData["MusicianID"] = new SelectList(_context.Musician, "ID", "Name", song.MusicianID);
            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SongID,Title,MusicianID,AlbumID")] Song song)
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
            ViewData["AlbumID"] = new SelectList(_context.Album, "ID", "Name", song.AlbumID);
            ViewData["MusicianSsn"] = new SelectList(_context.Musician, "ID", "Name", song.MusicianID);
            return View(song);
        }

        // GET: Songs/Delete/5
        [Authorize]
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
        [Authorize]
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
