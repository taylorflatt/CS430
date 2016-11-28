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
            int pageSize = 5;

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
            {
                return NotFound();
            }

            var place = await _context.Place
                .Include(p => p.Telephone)
                .Include(m => m.Musicians)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }

        // GET: Places/Create
        public IActionResult Create()
        {
            ViewData["TelephoneNumber"] = new SelectList(_context.Telephone, "Number", "Number");
            return View();
        }

        // POST: Places/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Address,TelephoneNumber")] Place model)
        {
            var uniqueAddress = from a in _context.Place
                                where a.Address == model.Address
                                select a.Address;

            var uniqueTelephone = from t in _context.Telephone
                                  where t.Number == model.TelephoneNumber
                                  select t.Place.Address;

            if (uniqueAddress.Any())
                ModelState.AddModelError("", "That address already exists.");

            if (uniqueTelephone.Any())
                ModelState.AddModelError("", "That telephone number already belongs to " + uniqueTelephone.First() + ". To assign number: " + model.TelephoneNumber
                    + " to this address, the " + uniqueTelephone.First() + " must first be deleted.");

            var place = new Place();
            var telephone = new Telephone();

            place.Address = model.Address;
            place.TelephoneNumber = model.TelephoneNumber;

            telephone.Number = model.TelephoneNumber;

            if (ModelState.IsValid)
            {
                // Need to add telephone first.
                _context.Add(telephone);
                _context.SaveChanges();

                _context.Add(place);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewData["TelephoneNumber"] = new SelectList(_context.Telephone, "Number", "Number", place.TelephoneNumber);
            return View(model);
        }

        // GET: Places/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.Place.SingleOrDefaultAsync(m => m.ID == id);
            if (place == null)
            {
                return NotFound();
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


            //ViewData["TelephoneNumber"] = new SelectList(_context.Telephone, "Number", "Number", place.TelephoneNumber);
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
            {
                return NotFound();
            }

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
            ViewData["TelephoneNumber"] = new SelectList(_context.Telephone, "Number", "Number", place.TelephoneNumber);
            return View(place);
        }

        // GET: Places/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.Place
                .Include(p => p.Telephone)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }

        // POST: Places/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var place = await _context.Place.Include(t => t.Telephone).SingleOrDefaultAsync(m => m.ID == id);
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
