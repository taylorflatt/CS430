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
                .SingleOrDefaultAsync(m => m.ID == id);

            if (musician == null)
            {
                return NotFound();
            }

            return View(musician);
        }

        // GET: Musicians/Create
        public IActionResult Create()
        {
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "ID", "Name");
            ViewData["PlaceID"] = new SelectList(_context.Place, "ID", "Address");
            return View();
        }

        // POST: Musicians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ssn,Name,PlaceID,InstrumentID")] Musician musician)
        {
            var uniqueSsn = from p in _context.Musician
                            where p.Ssn == musician.Ssn
                            select p.Name;

            if (uniqueSsn.Any())
                ModelState.AddModelError("", "That SSN (" + musician.Ssn + ") already belongs to " + uniqueSsn.First() + ". To assign this SSN to " 
                    + musician.Name + ", " + uniqueSsn.First() + " must first be deleted.");

            if (ModelState.IsValid)
            {
                _context.Add(musician);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "ID", "Name", musician.InstrumentID);
            ViewData["PlaceID"] = new SelectList(_context.Place, "ID", "Address", musician.PlaceID);
            return View(musician);
        }

        // GET: Musicians/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musician = await _context.Musician.SingleOrDefaultAsync(m => m.ID == id);
            if (musician == null)
            {
                return NotFound();
            }
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "ID", "Name", musician.InstrumentID);
            ViewData["PlaceID"] = new SelectList(_context.Place, "ID", "Address", musician.PlaceID);
            return View(musician);
        }

        // POST: Musicians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ssn,Name,PlaceID,InstrumentID")] Musician musician)
        {
            if (id != musician.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(musician).State = EntityState.Modified;
                    _context.Update(musician);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicianExists(musician.Ssn))
                        return NotFound();

                    else
                        throw;
                }
                return RedirectToAction("Index");
            }
            ViewData["InstrumentID"] = new SelectList(_context.Instrument, "ID", "Name", musician.InstrumentID);
            ViewData["PlaceID"] = new SelectList(_context.Place, "ID", "Address", musician.PlaceID);
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
                .SingleOrDefaultAsync(m => m.ID == id);
            if (musician == null)
            {
                return NotFound();
            }

            return View(musician);
        }

        // POST: Musicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musician = await _context.Musician.SingleOrDefaultAsync(m => m.ID == id);
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
