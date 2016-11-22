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
        private readonly ApplicationDbContext _context;

        public MusiciansController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Musicians
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Musicians.Include(m => m.Place);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Musicians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicians = await _context.Musicians.SingleOrDefaultAsync(m => m.id == id);

            foreach(var song in _context.Songs)
            {
                if (song.musicianIdForeignKey == song.songId)
                    musicians.Song.Add(song);
            }

            if (musicians == null)
            {
                return NotFound();
            }

            //ViewData["songForeignKey"] = new SelectList(_context.Songs, "songId", "title");
            return View(musicians);
        }

        // GET: Musicians/Create
        public IActionResult Create()
        {
            ViewData["placeForeignKey"] = new SelectList(_context.Place, "address", "address");
            return View();
        }

        // POST: Musicians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,placeForeignKey,ssn")] Musicians musicians)
        {
            if (ModelState.IsValid)
            {
                _context.Add(musicians);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["placeForeignKey"] = new SelectList(_context.Place, "address", "address", musicians.placeForeignKey);
            return View(musicians);
        }

        // GET: Musicians/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicians = await _context.Musicians.SingleOrDefaultAsync(m => m.id == id);
            if (musicians == null)
            {
                return NotFound();
            }
            ViewData["placeForeignKey"] = new SelectList(_context.Place, "address", "address", musicians.placeForeignKey);
            return View(musicians);
        }

        // POST: Musicians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,placeForeignKey,ssn")] Musicians musicians)
        {
            if (id != musicians.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(musicians);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusiciansExists(musicians.id))
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
            ViewData["placeForeignKey"] = new SelectList(_context.Place, "address", "address", musicians.placeForeignKey);
            return View(musicians);
        }

        // GET: Musicians/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicians = await _context.Musicians.SingleOrDefaultAsync(m => m.id == id);
            if (musicians == null)
            {
                return NotFound();
            }

            return View(musicians);
        }

        // POST: Musicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musicians = await _context.Musicians.SingleOrDefaultAsync(m => m.id == id);
            _context.Musicians.Remove(musicians);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MusiciansExists(int id)
        {
            return _context.Musicians.Any(e => e.id == id);
        }
    }
}
