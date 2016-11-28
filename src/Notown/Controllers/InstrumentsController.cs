using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notown.Data;
using Notown.Models;
using Microsoft.AspNetCore.Authorization;
using Notown.Helpers;

namespace Notown.Controllers
{
    [Authorize]
    public class InstrumentsController : Controller
    {
        private readonly NotownContext _context;

        public InstrumentsController(NotownContext context)
        {
            _context = context;    
        }

        // GET: Instruments
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageSize = 10;

            ViewData["CurrentSort"] = sortOrder;    // Allows us to keep sort order in paging links.
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["KeySortParm"] = sortOrder == "Key" ? "key_desc" : "Key";

            // Need to reset paging data because there is new information to display.
            if (searchString != null)
                page = 1;

            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;   // Allows us to keep filters in paging links.

            var instruments = from a in _context.Instrument
                            select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                instruments = instruments.Where(a => a.Name.Contains(searchString) ||
                    a.Key.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    instruments = instruments.OrderByDescending(a => a.Name);
                    break;
                case "Key":
                    instruments = instruments.OrderBy(a => a.Key);
                    break;
                case "key_desc":
                    instruments = instruments.OrderByDescending(a => a.Key);
                    break;
                default:
                    instruments = instruments.OrderBy(a => a.Name);
                    break;
            }

            return View(await PaginatedList<Instrument>.CreateAsync(instruments.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Instruments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var instrument = await _context.Instrument
                .Include(m => m.Musicians)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (instrument == null)
                return NotFound();

            return View(instrument);
        }

        // GET: Instruments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instruments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Key")] Instrument instrument)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instrument);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(instrument);
        }

        // GET: Instruments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var instrument = await _context.Instrument.SingleOrDefaultAsync(m => m.ID == id);

            if (instrument == null)
                return NotFound();

            return View(instrument);
        }

        // POST: Instruments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Key")] Instrument instrument)
        {
            if (id != instrument.ID)
                return NotFound();

            var uniqueName = from n in _context.Instrument
                             where n.Name.Equals(instrument.Name)
                             select 1;

            if(uniqueName.Any())
                ModelState.AddModelError("", "An instrument with that name already exists!");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instrument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstrumentExists(instrument.ID))
                        return NotFound();

                    else
                        throw;
                }
                return RedirectToAction("Index");
            }

            return View(instrument);
        }

        // GET: Instruments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var instrument = await _context.Instrument
                .Include(m => m.Musicians)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (instrument == null)
                return NotFound();

            return View(instrument);
        }

        // POST: Instruments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instrument = await _context.Instrument.Include(m => m.Musicians).SingleOrDefaultAsync(m => m.ID == id);

            if (instrument.Musicians.Count() > 0)
            {
                foreach (var musician in instrument.Musicians)
                {
                    var tempMusician = await _context.Musician.Include(s => s.Songs).SingleOrDefaultAsync(m => m.ID == musician.ID);
                    var songList = new List<Song>();

                    // Remove all the songs associated with the musician.
                    foreach (var song in tempMusician.Songs)
                    {
                        _context.Song.Remove(song);
                    }

                    _context.Musician.Remove(tempMusician);
                }

                _context.SaveChanges();
            }

            _context.Instrument.Remove(instrument);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private bool InstrumentExists(int id)
        {
            return _context.Instrument.Any(e => e.ID == id);
        }
    }
}
