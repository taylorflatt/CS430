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
    public class AlbumsController : Controller
    {
        private readonly NotownContext _context;

        public AlbumsController(NotownContext context)
        {
            _context = context;    
        }

        // GET: Albums
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageSize = 10;

            ViewData["CurrentSort"] = sortOrder;    // Allows us to keep sort order in paging links.
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["ProducerSortParm"] = sortOrder == "Producer" ? "producer_desc" : "Producer";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            // Need to reset paging data because there is new information to display.
            if (searchString != null)
                page = 1;

            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;   // Allows us to keep filters in paging links.

            var albums = from a in _context.Album.Include(a => a.Musician)
                         select a;

            if(!String.IsNullOrEmpty(searchString))
            {
                albums = albums.Where(a => a.Name.Contains(searchString) ||
                    a.Musician.Name.Contains(searchString));
            }

            switch(sortOrder)
            {
                case "title_desc":
                    albums = albums.OrderByDescending(a => a.Name);
                    break;
                case "Producer":
                    albums = albums.OrderBy(a => a.Musician.Name);
                    break;
                case "producer_desc":
                    albums = albums.OrderByDescending(a => a.Musician.Name);
                    break;
                case "Date":
                    albums = albums.OrderBy(a => a.CopyrightDate);
                    break;
                case "date_desc":
                    albums = albums.OrderByDescending(a => a.CopyrightDate);
                    break;
                default:
                    albums = albums.OrderBy(a => a.Name);
                    break;
            }

            return View(await PaginatedList<Album>.CreateAsync(albums.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Albums/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var album = await _context.Album
                .Include(a => a.Musician)
                .Include(s => s.Songs)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (album == null)
                return NotFound();

            return View(album);
        }

        // GET: Albums/Create
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var album = await _context.Album.SingleOrDefaultAsync(m => m.ID == id);

            if (album == null)
                return NotFound();

            List<SelectListItem> temp = new List<SelectListItem>();

            foreach (var musician in _context.Musician)
            {
                temp.Add(new SelectListItem
                {
                    Text = musician.Name + " (" + musician.Ssn + ")",
                    Value = Convert.ToString(musician.ID)
                });
            }

            ViewData["MusicianID"] = temp;

            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Speed,CopyrightDate,MusicianID")] Album album)
        {
            if (id != album.ID)
                return NotFound();

            var uniqueName = from n in _context.Album
                             where n.Name.Equals(album.Name)
                             select n.ID;

            // If there is an instrument and it isn't the same instrument.
            if (!uniqueName.FirstOrDefault().Equals(album.ID) && uniqueName.Count() > 0)
                ModelState.AddModelError("", "An album with that name already exists!");

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
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Index");
            }

            List<SelectListItem> temp = new List<SelectListItem>();

            foreach (var musician in _context.Musician)
            {
                temp.Add(new SelectListItem
                {
                    Text = musician.Name + " (" + musician.Ssn + ")",
                    Value = Convert.ToString(musician.ID)
                });
            }

            ViewData["MusicianID"] = temp;

            return View(album);
        }

        // GET: Albums/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var album = await _context.Album
                .Include(a => a.Musician)
                .Include(s => s.Songs)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (album == null)
                return NotFound();

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
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
