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
            List<SelectListItem> instrumentList = new List<SelectListItem>();
            List<SelectListItem> placeList = new List<SelectListItem>();

            // Default Option
            // Note: The Text option must be exactly this or else it needs changed in the view.
            instrumentList.Add(new SelectListItem
            {
                Text = "Create New...",
                Value = "-1"
            });

            placeList.Add(new SelectListItem
            {
                Text = "Create New...",
                Value = "-1"
            });

            foreach (var tempInstrument in _context.Instrument)
            {
                instrumentList.Add(new SelectListItem
                {
                    Text = tempInstrument.Name,
                    Value = Convert.ToString(tempInstrument.ID)
                });
            }

            foreach (var tempPlace in _context.Place)
            {
                placeList.Add(new SelectListItem
                {
                    Text = tempPlace.Address,
                    Value = Convert.ToString(tempPlace.ID)
                });
            }

            ViewData["InstrumentID"] = instrumentList;
            ViewData["PlaceID"] = placeList;
            return View();
        }

        // POST: Musicians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Instrument, Musician, Place, Telephone")] CreateMusicianViewModel model)
        {
            bool createNewInstrument = false;
            bool createNewPlace = false;

            if (model.Musician.InstrumentID == -1)
                createNewInstrument = true;

            if (model.Musician.PlaceID == -1)
                createNewPlace = true;

            if (string.IsNullOrEmpty(model.Musician.Name))
                ModelState.AddModelError("", "You must input a name for the artist.");

            if (string.IsNullOrEmpty(model.Musician.Ssn))
                ModelState.AddModelError("", "You must input an SSN for the artist.");

            var uniqueSsn = from p in _context.Musician
                            where p.Ssn == model.Musician.Ssn
                            select p.Name;

            if (uniqueSsn.Any())
                ModelState.AddModelError("", "That SSN (" + model.Musician.Ssn + ") already belongs to " + uniqueSsn.First() + ". To assign this SSN to "
                    + model.Musician.Name + ", " + uniqueSsn.First() + " must first be deleted.");

            if (createNewInstrument)
            {
                var uniqueInstruName = from i in _context.Instrument
                                     where i.Name == model.Instrument.Name
                                     select i.Name;

                if (uniqueInstruName.Any())
                    ModelState.AddModelError("", "That instrument name already exists. It must first be removed before you can add it. ");
            }

            if(createNewPlace)
            {
                var uniqueAddress = from a in _context.Place
                                    where a.Address == model.Place.Address
                                    select a.Address;

                var uniqueTelephone = from t in _context.Telephone
                                      where t.Number == model.Place.TelephoneNumber
                                      select t.Place.Address;

                if (uniqueAddress.Any())
                    ModelState.AddModelError("", "That address already exists. You may add this musician to that artist by choosing it in the dropdown list rather than creating "
                        + "a new address.");

                if (uniqueTelephone.Any())
                    ModelState.AddModelError("", "That telephone number already belongs to " + uniqueTelephone.First() + ". To assign number: " + model.Place.TelephoneNumber
                        + " to this address, the " + uniqueTelephone.First() + " must first be deleted.");

                if (string.IsNullOrEmpty(model.Place.TelephoneNumber))
                    ModelState.AddModelError("", "You must input a telephone number for the artist.");

                if (string.IsNullOrEmpty(model.Place.Address))
                    ModelState.AddModelError("", "You must input an address for the artist.");
            }

            var instrument = new Instrument();
            var musician = new Musician();
            var place = new Place();
            var telephone = new Telephone();

            try
            {
                if (createNewInstrument)
                {
                    instrument.Key = model.Instrument.Key;
                    instrument.Name = model.Instrument.Name;

                    if (ModelState.IsValid)
                    {
                        _context.Add(instrument);
                        _context.SaveChanges();
                    }

                    var instruID = from i in _context.Instrument
                                   where i.ID == instrument.ID
                                   select i.ID;

                    if (instruID.Count() == 1)
                        musician.InstrumentID = instruID.SingleOrDefault();
                    else
                        throw new Exception();
                }

                else
                    musician.InstrumentID = model.Musician.InstrumentID;

                if (createNewPlace)
                {
                    //var removeChars = new string[] { "(", ")", "-", " " };

                    //foreach (var ch in removeChars)
                    //{
                    //    place.TelephoneNumber = place.TelephoneNumber.Replace(ch, string.Empty);
                    //}

                    place.Address = model.Place.Address;
                    place.TelephoneNumber = model.Place.TelephoneNumber;

                    telephone.Number = model.Place.TelephoneNumber;

                    if (ModelState.IsValid)
                    {
                        _context.Add(telephone);
                        _context.SaveChanges();

                        _context.Add(place);
                        _context.SaveChanges();
                    }

                    var placeID = from i in _context.Place
                                  where i.ID == place.ID
                                  select i.ID;

                    if (placeID.Count() == 1)
                        musician.PlaceID = placeID.SingleOrDefault();
                    else
                        throw new Exception();
                }

                else
                    musician.PlaceID = model.Musician.PlaceID;

                musician.Name = model.Musician.Name;
                musician.Ssn = model.Musician.Ssn;

                if (ModelState.IsValid)
                {
                    _context.Add(musician);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }

                else
                    throw new Exception();
            }
            catch(Exception)
            {
                if (createNewInstrument)
                {
                    // Remove any new instrument added to the database.
                    var tempInstrument = from i in _context.Instrument
                                         where i.ID == instrument.ID
                                         select i.ID;

                    if (tempInstrument.Any())
                    {
                        _context.Remove(tempInstrument);
                        _context.SaveChanges();
                    }
                }

                if (createNewPlace)
                {
                    // Remove any new place added to the database.
                    var tempPlace = from i in _context.Place
                                         where i.ID == place.ID
                                         select i.ID;

                    if (tempPlace.Any())
                    {
                        _context.Remove(tempPlace);
                        _context.SaveChanges();
                    }
                }

                List<SelectListItem> instrumentList = new List<SelectListItem>();
                List<SelectListItem> placeList = new List<SelectListItem>();

                // Default Option
                // Note: The Text option must be exactly this or else it needs changed in the view.
                instrumentList.Add(new SelectListItem
                {
                    Text = "Create New...",
                    Value = "-1"
                });

                placeList.Add(new SelectListItem
                {
                    Text = "Create New...",
                    Value = "-1"
                });

                foreach (var tempInstrument in _context.Instrument)
                {
                    instrumentList.Add(new SelectListItem
                    {
                        Text = tempInstrument.Name,
                        Value = Convert.ToString(tempInstrument.ID)
                    });
                }

                foreach (var tempPlace in _context.Place)
                {
                    placeList.Add(new SelectListItem
                    {
                        Text = tempPlace.Address,
                        Value = Convert.ToString(tempPlace.ID)
                    });
                }

                ViewData["InstrumentID"] = instrumentList;
                ViewData["PlaceID"] = placeList;

                return View(model);
            }
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
