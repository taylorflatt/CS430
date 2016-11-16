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
            return View(await _context.Musicians.ToListAsync());
        }

        // GET: Musicians/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Musicians/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Musicians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ssn,name")] Musicians musicians)
        {
            if (ModelState.IsValid)
            {
                _context.Add(musicians);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
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
            return View(musicians);
        }

        // POST: Musicians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ssn,name")] Musicians musicians)
        {
            if (id != musicians.ssn)
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
                    if (!MusiciansExists(musicians.ssn))
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
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var musicians = await _context.Musicians.SingleOrDefaultAsync(m => m.ssn == id);
            _context.Musicians.Remove(musicians);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MusiciansExists(string id)
        {
            return _context.Musicians.Any(e => e.ssn == id);
        }
    }
}
