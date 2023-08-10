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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceRepository.GetAllRaces();
            return View(races);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceRepository.GetRaceById(id);
            return View(race);
        }

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetRaceByIdNoTracking(id);

            if (race == null) return View("Error");

            var oldRace = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                RaceCategory = race.RaceCategory,
                AddressId = race.AddressId,
                Address = race.Address,
            };
            return View(oldRace);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel newRace)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", newRace);
            }

            var race = await _raceRepository.GetRaceByIdNoTracking(id);

            if (race == null)
            {
                return View("Error");
            }

            var photoResult = await _photoService.AddPhotoAsync(newRace.Image);

            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(newRace);
            }

            if (!string.IsNullOrEmpty(race.Image))
            {
                _ = _photoService.DeletePhotoAsync(race.Image);
            }

            var updateRace = new Race
            {
                Id = id,
                Title = newRace.Title,
                Description = newRace.Description,
                Image = photoResult.Url.ToString(),
                AddressId = newRace.AddressId,
                Address = newRace.Address,
            };

            _raceRepository.Update(updateRace);

            return RedirectToAction("Index");
        }
    }
}
