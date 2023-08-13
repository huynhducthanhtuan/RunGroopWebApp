using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Helpers;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repositories;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RaceController(
            IRaceRepository raceRepository, 
            IPhotoService photoService,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _raceRepository = raceRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
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
            if (race == null)
            {
                TempData["Error"] = "This race is not found!";
                return View("Error");
            }
            return View(race);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createRaceViewModel = new CreateRaceViewModel()
            {
                AppUserId = userId
            };
            return View(createRaceViewModel);
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
                    },
                    AppUserId = newRace.AppUserId
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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Race race = await _raceRepository.GetRaceById(id);
            if (race == null) return View("Error");

            _raceRepository.Delete(race);
            return RedirectToAction("Index");
        }
    }
}
