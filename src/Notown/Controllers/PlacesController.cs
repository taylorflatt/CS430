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
    public class PlacesController : Controller
    {
        private readonly NotownContext _context;

        public PlacesController(NotownContext context)
        {
            _context = context;    
        }

        // GET: Places
        public async Task<IActionResult> Index()
        {
            var notownContext = _context.Place.Include(p => p.Telephone);
            return View(await notownContext.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Address,TelephoneNumber")] Place place)
        {
            foreach(var phone in _context.Telephone)
            {
                if (phone.Number.Equals(place.TelephoneNumber))
                {
                    ModelState.AddModelError("", "That number already exists in the database!");
                    break;
                }
            }

            var telephoneModel = new Telephone();
            telephoneModel.Number = place.TelephoneNumber;

            if (ModelState.IsValid)
            {
                _context.Add(telephoneModel);
                _context.Add(place);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            //ViewData["TelephoneNumber"] = new SelectList(_context.Telephone, "Number", "Number", place.TelephoneNumber);
            return View(place);
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
            ViewData["TelephoneNumber"] = new SelectList(_context.Telephone, "Number", "Number", place.TelephoneNumber);
            return View(new EditPlaceViewModel { Address = place.Address, TelephoneNumber = place.TelephoneNumber });
        }

        // POST: Places/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Address,TelephoneNumber")] EditPlaceViewModel place)
        {
            var uuid = from p in _context.Place
                       where p.ID == id
                       select p.ID;

            if (id != uuid.SingleOrDefault())
                return NotFound();

            var numberExists = from n in _context.Telephone
                               where n.Number == place.TelephoneNumber
                               select place.Address;

            if (numberExists.Any())
                ModelState.AddModelError("", numberExists.FirstOrDefault() + " already has that number!");

            else
            {
                var newNumber = new Telephone();
                var oldNumber = new Telephone();
                var removeChars = new string[] { "(", ")", "-", " " };

                foreach(var ch in removeChars)
                {
                    place.TelephoneNumber = place.TelephoneNumber.Replace(ch, string.Empty);
                }

                newNumber.Number = place.TelephoneNumber;

                if (!newNumber.Number.Equals(place.TelephoneNumber))
                    oldNumber = _context.Telephone.SingleOrDefault(p => p.Place.ID == uuid.FirstOrDefault());

                var model = new Place();
                model.Address = place.Address;
                model.TelephoneNumber = place.TelephoneNumber;

                if (ModelState.IsValid)
                {
                    try
                    {
                        // Add new phone number to the database.
                        if (oldNumber.Number != null)
                        {
                            _context.Add(newNumber);
                            _context.SaveChanges();
                        }

                        _context.Entry(model).State = EntityState.Modified;
                        _context.Update(model);

                        // Remove the old phone number from the database.
                        if (oldNumber.Number != null)
                        {
                            _context.Remove(oldNumber);
                            _context.SaveChanges();
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PlaceExists(place.Address))
                            return NotFound();

                        else
                            throw;
                    }
                    return RedirectToAction("Index");
                }
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
            var place = await _context.Place.SingleOrDefaultAsync(m => m.ID == id);
            _context.Place.Remove(place);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PlaceExists(string id)
        {
            return _context.Place.Any(e => e.Address == id);
        }
    }
}
