using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repositories;
using RunGroopWebApp.Services;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
            _raceRepository = raceRepository;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceRepository.GetAllRaces();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceRepository.GetRaceById(id);
            return View(race);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel newRace)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(newRace.Image);

                var race = new Race
                {
                    Title = newRace.Title,
                    Description = newRace.Description,
                    Image = result.Url.ToString(),
                    RaceCategory = newRace.RaceCategory,
                    // AppUserId = newRace.AppUserId,
                    Address = new Address
                    {
                        Street = newRace.Address.Street,
                        City = newRace.Address.City,
                        State = newRace.Address.State,
                    }
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(newRace);
        }
    }
}
