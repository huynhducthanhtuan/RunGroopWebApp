using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroup.Helpers;
using RunGroup.Interfaces;
using RunGroup.Models;
using RunGroup.ViewModels;

namespace RunGroup.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClubController(
            IClubRepository clubRepository,
            IPhotoService photoService,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAllClubs();
            return View(clubs);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            ClubViewModel club = await _clubRepository.GetClubById(id);
            if (club == null)
            {
                TempData["Error"] = "This club is not found!";
                return View("Error");
            }
            return View(club);
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

            var createClubViewModel = new CreateClubViewModel()
            {
                AppUserId = userId
            };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel club)
        {
            if (ModelState.IsValid)
            {
                ImageUploadResult result = await _photoService.AddPhotoAsync(club.Image);

                Club newClub = new Club
                {
                    Title = club.Title,
                    Description = club.Description,
                    Image = result.Url.ToString(),
                    ClubCategory = club.ClubCategory,
                    Address = new Address
                    {
                        Street = club.Address.Street,
                        City = club.Address.City,
                        State = club.Address.State,
                    },
                    AppUserId = club.AppUserId
                };

                _clubRepository.Add(newClub);

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(club);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ClubViewModel club = await _clubRepository.GetClubById(id);
            if (club == null) return View("Error");

            EditClubViewModel oldClub = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                ClubCategory = club.ClubCategory,
                AddressId = (int)club.AddressId,
                Address = new Address()
                {
                    Id = (int)club.AddressId,
                    Street = club.AddressStreet,
                    City = club.AddressCity,
                    State = club.AddressState
                }
            };
            return View(oldClub);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel newClub)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", newClub);
            }

            ClubViewModel club = await _clubRepository.GetClubById(id);
            if (club == null) return View("Error");

            ImageUploadResult photoResult = await _photoService.AddPhotoAsync(newClub.Image);
            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(newClub);
            }
            if (!string.IsNullOrEmpty(club.Image))
            {
                _ = _photoService.DeletePhotoAsync(club.Image);
            }

            Club updateClub = new Club
            {
                Id = newClub.Id,
                Title = newClub.Title,
                Description = newClub.Description,
                Image = photoResult.Url.ToString(),
                ClubCategory = newClub.ClubCategory,
                AddressId = club.AddressId,
                Address = new Address()
                {
                    Id = (int)club.AddressId,
                    Street = newClub.Address.Street,
                    City = newClub.Address.City,
                    State = newClub.Address.State
                }
            };
            await _clubRepository.Update(updateClub);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ClubViewModel club = await _clubRepository.GetClubById(id);
            if (club == null) return View("Error");

            bool isDeleted = await _clubRepository.Delete(id);
            if (isDeleted == false) return View("Error");

            return RedirectToAction("Index");
        }
    }
}
