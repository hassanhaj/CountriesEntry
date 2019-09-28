using CountriesEntry.Entities;
using CountriesEntry.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountriesEntry.Controllers
{
    [Authorize]
    //[ValidateCountry]
    public class CountriesController : Controller
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Executing
            var param = context.ActionArguments;
            await base.OnActionExecutionAsync(context, next);
            // Executed
            var result = context.Result as ViewResult;
            routeData = context.RouteData.ToString();
        }

        private readonly AppDbContext _context;
        private readonly RequestInfo _requestInfo;
        private string routeData = null;

        public CountriesController(AppDbContext context, RequestInfo requestInfo)
        {
            _context = context;
            this._requestInfo = requestInfo;
        }

        // GET: Countries
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 60 * 60)]
        public async Task<IActionResult> Index(string name, string countryCode)
        {
            var result = await _context.Countries
                .Where(e => string.IsNullOrEmpty(name) || e.Name.Contains(name))
                .ToListAsync();

            var model = new SearchCountry
            {
                Countries = result
            };

            return View(model);
        }

        // GET: Countries/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .Include(e => e.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);

            ViewData["username"] = User.Identity.Name;
            ViewData["Cities"] = country.Cities;

            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Countries/Create
        [AddHeaderFilter]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[ValidateCountry(]
        public async Task<IActionResult> Create([Bind("Id,Name,CountryCode")] Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CountryCode")] Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        public IActionResult GetContent()
        {
            return new ContentResult
            {
              Content =  "Country is Jordan",
              ContentType = "text",
              StatusCode = 200
            };
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(int id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }
    }
}
