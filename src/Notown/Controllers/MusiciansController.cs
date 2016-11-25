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
    public class MusiciansController : Controller
    {
        private readonly NotownContext _context;

        public MusiciansController(NotownContext context)
        {
            _context = context;    
        }

        // GET: Musicians
        public async Task<IActionResult> Index()
        {
            var notownContext = _context.Musician.Include(m => m.Instrument).Include(m => m.Place);
            return View(await notownContext.ToListAsync());
        }

        // GET: Musicians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musician = await _context.Musician
                .Include(m => m.Instrument)
                .Include(m => m.Place)
                .Include(m => m.Songs)
                .Include(m => m.Albums)
                .SingleOrDefaultAsync(m => m.uniqueID == id);

            if (musician == null)
            {
                return NotFound();
            }

            return View(musician);
        }

        // GET: Musicians/Create
        public IActionResult Create()
        {
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "ID", "ID");
            ViewData["PlaceAddress"] = new SelectList(_context.Place, "Address", "Address");
            return View();
        }

        // POST: Musicians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ssn,Name,PlaceAddress,InstrumentID")] Musician musician)
        {
            if (ModelState.IsValid)
            {
                _context.Add(musician);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "ID", "ID", musician.InstrumentID);
            ViewData["PlaceAddress"] = new SelectList(_context.Place, "Address", "Address", musician.PlaceAddress);
            return View(musician);
        }

        // GET: Musicians/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musician = await _context.Musician.SingleOrDefaultAsync(m => m.uniqueID == id);
            if (musician == null)
            {
                return NotFound();
            }
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "ID", "ID", musician.InstrumentID);
            ViewData["PlaceAddress"] = new SelectList(_context.Place, "Address", "Address", musician.PlaceAddress);
            return View(musician);
        }

        // POST: Musicians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id?, [Bind("Ssn,Name,PlaceAddress,InstrumentID")] Musician musician)
        {
            if (id != musician.uniqueID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(musician);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicianExists(musician.Ssn))
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
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "ID", "ID", musician.InstrumentID);
            ViewData["PlaceAddress"] = new SelectList(_context.Place, "Address", "Address", musician.PlaceAddress);
            return View(musician);
        }

        // GET: Musicians/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musician = await _context.Musician
                .Include(m => m.Instrument)
                .Include(m => m.Place)
                .SingleOrDefaultAsync(m => m.uniqueID == id);
            if (musician == null)
            {
                return NotFound();
            }

            return View(musician);
        }

        // POST: Musicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var musician = await _context.Musician.SingleOrDefaultAsync(m => m.uniqueID == id);
            _context.Musician.Remove(musician);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MusicianExists(string id)
        {
            return _context.Musician.Any(e => e.Ssn == id);
        }
    }
}
