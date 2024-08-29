using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroup.Helpers;
using RunGroup.Interfaces;
using RunGroup.Models;
using RunGroup.ViewModels;

namespace RunGroup.Controllers
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
            RaceViewModel race = await _raceRepository.GetRaceById(id);
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
            if (userId == null)
            {
                TempData["Error"] = "Please log in to continue!";
                return View("Error");
            }

            var createRaceViewModel = new CreateRaceViewModel()
            {
                AppUserId = userId
            };
            return View(createRaceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel race)
        {
            if (ModelState.IsValid)
            {
                ImageUploadResult result = await _photoService.AddPhotoAsync(race.Image);
                Race newRace = new Race
                {
                    Title = race.Title,
                    Description = race.Description,
                    Image = result.Url.ToString(),
                    RaceCategory = race.RaceCategory,
                    Address = new Address
                    {
                        Street = race.Address.Street,
                        City = race.Address.City,
                        State = race.Address.State,
                    },
                    AppUserId = race.AppUserId
                };

                _raceRepository.Add(newRace);

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(race);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            RaceViewModel race = await _raceRepository.GetRaceById(id);
            if (race == null) return View("Error");

            EditRaceViewModel oldRace = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                RaceCategory = race.RaceCategory,
                AddressId = (int)race.AddressId,
                Address = new Address()
                {
                    Id = (int)race.AddressId,
                    Street = race.AddressStreet,
                    City = race.AddressCity,
                    State = race.AddressState
                }
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

            RaceViewModel race = await _raceRepository.GetRaceById(id);
            if (race == null) return View("Error");

            ImageUploadResult photoResult = await _photoService.AddPhotoAsync(newRace.Image);
            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(newRace);
            }
            if (!string.IsNullOrEmpty(race.Image))
            {
                _ = _photoService.DeletePhotoAsync(race.Image);
            }

            Race updateRace = new Race
            {
                Id = newRace.Id,
                Title = newRace.Title,
                Description = newRace.Description,
                Image = photoResult.Url.ToString(),
                AddressId = newRace.AddressId,
                Address = new Address()
                {
                    Id = (int)race.AddressId,
                    Street = newRace.Address.Street,
                    City = newRace.Address.City,
                    State = newRace.Address.State
                }
            };
            await _raceRepository.Update(updateRace);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            RaceViewModel race = await _raceRepository.GetRaceById(id);
            if (race == null) return View("Error");

            bool isDeleted = await _raceRepository.Delete(id);
            if (isDeleted == false) return View("Error");

            return RedirectToAction("Index");
        }
    }
}
