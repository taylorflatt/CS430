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
using Microsoft.AspNetCore.Mvc.Rendering;
using Notown.Models.NotownViewModels;

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
            List<SelectListItem> temp = new List<SelectListItem>();

            temp.Add(new SelectListItem
            {
                Text = "No Musician",
                Value = "-1",
                Selected = true
            });

            foreach (var musician in _context.Musician)
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

        // POST: Instruments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Instrument, MusicianIDs")] CreateInstrumentViewModel model)
        {
            if (model.MusicianIDs == null)
                ModelState.AddModelError("MusicianIDs", "You must select at least one option! If you want 'No Musicians', select that option.");

            var uniqueName = from k in _context.Instrument
                            where k.Name == model.Instrument.Name
                            select 1;

            if (uniqueName.Any())
                ModelState.AddModelError("Instrument.Name", "The instrument name has already been added to the database!");

            // They selected 'No Musician' AND a musician. Nonsensical choice.
            if (model.MusicianIDs != null && model.MusicianIDs.Contains(-1) && model.MusicianIDs.Count() > 1)
            {
                ModelState.AddModelError("MusicianIDs", "You selected 'No Musician' but also selected at least another musician as well. If you don't wish to add musicians to this" +
                    " instrument, then only select the 'No Musician' option. Otherwise, unselect 'No Musician' and choose as many musicians who will be assigned to this" +
                    " instrument.");
            }

            var instrument = new Instrument();
            instrument.Key = model.Instrument.Key;
            instrument.Name = model.Instrument.Name;

            if (ModelState.IsValid)
            {
                // Add the instrument first so the ID can be added to the musicians.
                _context.Add(instrument);
                await _context.SaveChangesAsync();

                if (!model.MusicianIDs.Contains(-1))
                {
                    foreach (var id in model.MusicianIDs)
                    {
                        var musician = _context.Musician.SingleOrDefault(m => m.ID == id);
                        musician.InstrumentID = _context.Instrument.SingleOrDefault(m => m.Name == model.Instrument.Name).ID;

                        _context.Musician.Update(musician);
                    }
                }

                // If there was an error, we need to remove the newly added instrument from the database and redisplay the form.
                if (!ModelState.IsValid)
                {
                    var newInstrument = _context.Instrument.SingleOrDefault(m => m.Name == model.Instrument.Name);
                    _context.Remove(newInstrument);

                    await _context.SaveChangesAsync();
                }

                // Only if completely successful
                else
                {
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }

            List<SelectListItem> temp = new List<SelectListItem>();

            temp.Add(new SelectListItem
            {
                Text = "No Musician",
                Value = "-1",
                Selected = true
            });

            foreach (var musician in _context.Musician)
            {
                temp.Add(new SelectListItem
                {
                    Text = musician.Name + " (" + musician.Ssn + ")",
                    Value = Convert.ToString(musician.ID)
                });
            }

            ViewData["MusicianID"] = temp;

            return View(model);
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
                             select n.ID;

            // If there is an instrument and it isn't the same instrument with the input name.
            if(!uniqueName.FirstOrDefault().Equals(instrument.ID) && uniqueName.Count() > 0)
                ModelState.AddModelError("Name", "An instrument with that name already exists!");

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
