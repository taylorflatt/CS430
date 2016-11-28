using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Notown.Data;
using Notown.Models;
using Microsoft.AspNetCore.Authorization;
using Notown.Helpers;
using Notown.Models.NotownViewModels;

namespace Notown.Controllers
{
    [Authorize]
    public class PlacesController : Controller
    {
        private readonly NotownContext _context;

        public PlacesController(NotownContext context)
        {
            _context = context;    
        }

        // GET: Places
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageSize = 10;

            ViewData["CurrentSort"] = sortOrder;    // Allows us to keep sort order in paging links.
            ViewData["AddressSortParm"] = String.IsNullOrEmpty(sortOrder) ? "address_desc" : "";

            // Need to reset paging data because there is new information to display.
            if (searchString != null)
                page = 1;

            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;   // Allows us to keep filters in paging links.

            var places = from t in _context.Place.Include(t => t.Telephone)
                         select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                places = places.Where(a => a.Address.Contains(searchString) ||
                    a.TelephoneNumber.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "address_desc":
                    places = places.OrderByDescending(a => a.Address);
                    break;
                default:
                    places = places.OrderBy(a => a.Address);
                    break;
            }

            return View(await PaginatedList<Place>.CreateAsync(places.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Places/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var place = await _context.Place
                .Include(p => p.Telephone)
                .Include(m => m.Musicians)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (place == null)
                return NotFound();

            return View(place);
        }

        // GET: Places/Create
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
            ViewData["TelephoneNumber"] = new SelectList(_context.Telephone, "Number", "Number");

            return View();
        }

        // POST: Places/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Place, MusicianIDs")] CreatePlaceViewModel model)
        {
            if (model.MusicianIDs == null)
                ModelState.AddModelError("MusicianIDs", "You must select at least one option! If you want 'No Musicians', select that option.");

            var uniqueAddress = from a in _context.Place
                                where a.Address == model.Place.Address
                                select a.Address;

            var uniqueTelephone = from t in _context.Telephone
                                  where t.Number == model.Place.TelephoneNumber
                                  select t.Place.Address;

            if (uniqueAddress.Any())
                ModelState.AddModelError("Place.Address", "That address already exists.");

            if (uniqueTelephone.Any())
                ModelState.AddModelError("Place.TelephoneNumber", "That telephone number already belongs to " + uniqueTelephone.First() + ". To assign number: " + model.Place.TelephoneNumber
                    + " to this address, the " + uniqueTelephone.First() + " must first be deleted.");

            // They selected 'No Musician' AND a musician. Nonsensical choice.
            if (model.MusicianIDs != null && model.MusicianIDs.Contains(-1) && model.MusicianIDs.Count() > 1)
            {
                ModelState.AddModelError("MusicianIDs", "You selected 'No Musician' but also selected at least another musician as well. If you don't wish to add musicians to this" +
                    " home, then only select the 'No Musician' option. Otherwise, unselect 'No Musician' and choose as many musicians who will be assigned to this" +
                    " home.");
            }

            var place = new Place();
            var telephone = new Telephone();

            place.Address = model.Place.Address;
            place.TelephoneNumber = model.Place.TelephoneNumber;

            telephone.Number = model.Place.TelephoneNumber;

            if (ModelState.IsValid)
            {
                // Need to add telephone first.
                _context.Add(telephone);
                _context.SaveChanges();

                _context.Add(place);
                _context.SaveChanges();

                if (!model.MusicianIDs.Contains(-1))
                {
                    foreach (var id in model.MusicianIDs)
                    {
                        var musician = _context.Musician.SingleOrDefault(m => m.ID == id);
                        musician.PlaceID = _context.Place.SingleOrDefault(m => m.Address == model.Place.Address).ID;

                        _context.Musician.Update(musician);
                    }
                }

                // If there was an error, we need to remove the newly added place and telephone from the database and redisplay the form.
                // This should never (or rarely) happen. This is only if something massively goes wrong in updating the musician PlaceID.
                if (!ModelState.IsValid)
                {
                    var newTelephone = _context.Telephone.SingleOrDefaultAsync(t => t.Number == model.Place.TelephoneNumber);
                    var newPlace = _context.Place.SingleOrDefaultAsync(p => p.Address == model.Place.Address);

                    _context.Remove(newTelephone);
                    _context.Remove(newPlace);

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
            ViewData["TelephoneNumber"] = new SelectList(_context.Telephone, "Number", "Number");

            return View(model);
        }

        // GET: Places/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var place = await _context.Place.SingleOrDefaultAsync(m => m.ID == id);

            if (place == null)
                return NotFound();

            List<SelectListItem> temp = new List<SelectListItem>();

            temp.Add(new SelectListItem
            {
                Text = place.TelephoneNumber + " - (Current)",
                Value = Convert.ToString(place.TelephoneNumber)
            });

            foreach (var number in _context.Telephone)
            {
                bool add = true;

                foreach(var tempPlace in _context.Place)
                {
                    // If a we find a number associated with another place, stop and don't add.
                    if (tempPlace.TelephoneNumber == number.Number)
                    {
                        add = false;
                        break;
                    }
                }

                if(add)
                {
                    temp.Add(new SelectListItem
                    {
                        Text = number.Number,
                        Value = Convert.ToString(number.Number)
                    });
                }
            }

            ViewData["TelephoneNumber"] = temp;

            return View(place);
        }

        // POST: Places/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Address,TelephoneNumber")] Place place)
        {
            if (id != place.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(place);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaceExists(place.ID))
                        return NotFound();

                    else
                        throw;
                }

                return RedirectToAction("Index");
            }

            List<SelectListItem> temp = new List<SelectListItem>();

            temp.Add(new SelectListItem
            {
                Text = place.TelephoneNumber + " - (Current)",
                Value = Convert.ToString(place.TelephoneNumber)
            });

            foreach (var number in _context.Telephone)
            {
                bool add = true;

                foreach (var tempPlace in _context.Place)
                {
                    // If a we find a number associated with another place, stop and don't add.
                    if (tempPlace.TelephoneNumber == number.Number)
                    {
                        add = false;
                        break;
                    }
                }

                if (add)
                {
                    temp.Add(new SelectListItem
                    {
                        Text = number.Number,
                        Value = Convert.ToString(number.Number)
                    });
                }
            }

            ViewData["TelephoneNumber"] = temp;

            return View(place);
        }

        // GET: Places/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var place = await _context.Place
                .Include(m => m.Musicians)
                .Include(p => p.Telephone)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (place == null)
                return NotFound();

            return View(place);
        }

        // POST: Places/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var place = await _context.Place.Include(t => t.Telephone).Include(m => m.Musicians).SingleOrDefaultAsync(m => m.ID == id);

            // The place has musicians, so we need to remove them as well. This will also remove the Albums and Songs 
            // associated with the musician as well.
            if (place.Musicians.Count() > 0)
            {
                foreach (var musician in place.Musicians)
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

            _context.Place.Remove(place);
            _context.SaveChanges();

            var telephone = await _context.Telephone.SingleOrDefaultAsync(t => t.Number == place.TelephoneNumber);

            _context.Telephone.Remove(telephone);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private bool PlaceExists(int id)
        {
            return _context.Place.Any(e => e.ID == id);
        }
    }
}
