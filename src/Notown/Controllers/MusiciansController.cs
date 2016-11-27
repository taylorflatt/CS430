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
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageSize = 5;

            ViewData["CurrentSort"] = sortOrder;    // Allows us to keep sort order in paging links.
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["InstrumentSortParm"] = sortOrder == "Instrument" ? "instrument_desc" : "Instrument";
            ViewData["PlaceSortParm"] = sortOrder == "Place" ? "place_desc" : "Place";

            // Need to reset paging data because there is new information to display.
            if (searchString != null)
                page = 1;

            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;   // Allows us to keep filters in paging links.

            var musicians = from a in _context.Musician.Include(a => a.Place).Include(i => i.Instrument)
                            select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                musicians = musicians.Where(a => a.Name.Contains(searchString) ||
                    a.Place.Address.Contains(searchString) || a.Instrument.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    musicians = musicians.OrderByDescending(a => a.Name);
                    break;
                case "Instrument":
                    musicians = musicians.OrderBy(a => a.Instrument.Name);
                    break;
                case "instrument_desc":
                    musicians = musicians.OrderByDescending(a => a.Instrument.Name);
                    break;
                case "Place":
                    musicians = musicians.OrderBy(a => a.Place.Address);
                    break;
                case "place_desc":
                    musicians = musicians.OrderByDescending(a => a.Place.Address);
                    break;
                default:
                    musicians = musicians.OrderBy(a => a.Name);
                    break;
            }

            return View(await PaginatedList<Musician>.CreateAsync(musicians.AsNoTracking(), page ?? 1, pageSize));
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

        //// GET: Musicians/Create
        //public IActionResult Create()
        //{
        //    List<SelectListItem> temp = new List<SelectListItem>();

        //    // Default Option
        //    temp.Add(new SelectListItem
        //    {
        //        Text = "Create New",
        //        Value = "-1"
        //    });

        //    foreach (var instrument in _context.Instrument)
        //    {
        //        temp.Add(new SelectListItem
        //        {
        //            Text = instrument.Name,
        //            Value = Convert.ToString(instrument.ID)
        //        });
        //    }

        //    ViewData["InstrumentID"] = temp;

        //    //ViewData["InstrumentID"] = new SelectList(_context.Instrument, "ID", "Name");
        //    ViewData["PlaceID"] = new SelectList(_context.Place, "ID", "Address");
        //    return View();
        //}

        // GET: Musicians/Create
        public IActionResult Create()
        {
            List<SelectListItem> temp = new List<SelectListItem>();

            // Default Option
            // Note: The Text option must be exactly this or else it needs changed in the view.
            temp.Add(new SelectListItem
            {
                Text = "Create New...",
                Value = "-1"
            });

            foreach (var instrument in _context.Instrument)
            {
                temp.Add(new SelectListItem
                {
                    Text = instrument.Name,
                    Value = Convert.ToString(instrument.ID)
                });
            }

            ViewData["InstrumentID"] = temp;
            ViewData["PlaceID"] = new SelectList(_context.Place, "ID", "Address");
            return View();
        }

        // POST: Musicians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Instrument, Musician")] CreateMusicianViewModel model)
        {
            bool createNewInstrument = false;
            if (model.Musician.InstrumentID == -1)
                createNewInstrument = true;

            var uniqueSsn = from p in _context.Musician
                            where p.Ssn == model.Musician.Ssn
                            select p.Name;

            var uniqueInstruID = from i in _context.Instrument
                                 where i.ID == model.Instrument.ID || i.ID == model.Musician.InstrumentID
                                 select i.Name;

            if (uniqueSsn.Any())
                ModelState.AddModelError("", "That SSN (" + model.Musician.Ssn + ") already belongs to " + uniqueSsn.First() + ". To assign this SSN to "
                    + model.Musician.Name + ", " + uniqueSsn.First() + " must first be deleted.");

            if (uniqueInstruID.Any())
                ModelState.AddModelError("", "That ID already belongs to " + uniqueInstruID.First() + ". To assign ID: " + model.Instrument.ID + " to this instrument, the "
                    + uniqueInstruID.First() + " must first be deleted.");

            if (string.IsNullOrEmpty(model.Musician.Name))
                ModelState.AddModelError("", "You must input a name for the artist.");

            if(string.IsNullOrEmpty(model.Musician.Ssn))
                ModelState.AddModelError("", "You must input a SSN for the artist.");

            var instrument = new Instrument();
            var musician = new Musician();

            if (createNewInstrument)
            {
                instrument.ID = model.Instrument.ID;
                instrument.Key = model.Instrument.Key;
                instrument.Name = model.Instrument.Name;

                musician.InstrumentID = model.Instrument.ID;
            }

            else
                musician.InstrumentID = model.Musician.InstrumentID;


            musician.Name = model.Musician.Name;
            musician.PlaceID = model.Musician.PlaceID;
            musician.Ssn = model.Musician.Ssn;

            if (ModelState.IsValid)
            {
                if (createNewInstrument)
                {
                    _context.Add(instrument);
                    _context.SaveChanges();
                }

                _context.Add(musician);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            List<SelectListItem> temp = new List<SelectListItem>();

            // Default Option
            // Note: The Text option must be exactly this or else it needs changed in the view.
            temp.Add(new SelectListItem
            {
                Text = "Create New...",
                Value = "-1"
            });

            foreach (var tempInstrument in _context.Instrument)
            {
                temp.Add(new SelectListItem
                {
                    Text = tempInstrument.Name,
                    Value = Convert.ToString(tempInstrument.ID)
                });
            }

            ViewData["InstrumentID"] = temp;
            ViewData["PlaceID"] = new SelectList(_context.Place, "ID", "Address");
            return View(model);
        }

        //// POST: Musicians/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Ssn,Name,PlaceID,InstrumentID")] Musician musician)
        //{
        //    var uniqueSsn = from p in _context.Musician
        //                    where p.Ssn == musician.Ssn
        //                    select p.Name;

        //    if (uniqueSsn.Any())
        //        ModelState.AddModelError("", "That SSN (" + musician.Ssn + ") already belongs to " + uniqueSsn.First() + ". To assign this SSN to " 
        //            + musician.Name + ", " + uniqueSsn.First() + " must first be deleted.");

        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(musician);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewData["InstrumentID"] = new SelectList(_context.Instrument, "ID", "Name", musician.InstrumentID);
        //    ViewData["PlaceID"] = new SelectList(_context.Place, "ID", "Address", musician.PlaceID);
        //    return View(musician);
        //}

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
