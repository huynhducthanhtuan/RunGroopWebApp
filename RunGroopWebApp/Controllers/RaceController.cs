using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaceController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            List<Race> races = this._context.Races.ToList();
            return View(races);
        }

        public IActionResult Detail(int id)
        {
            Race race = this._context.Races
                .Include(a => a.Address)
                .FirstOrDefault(r => r.Id == id);
            return View(race);
        }
    }
}
