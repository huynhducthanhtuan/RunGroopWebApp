using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAllClubs();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubRepository.GetClubById(id);
            return View(club);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel newClub)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(newClub.Image);

                var club = new Club
                {
                    Title = newClub.Title,
                    Description = newClub.Description,
                    Image = result.Url.ToString(),
                    ClubCategory = newClub.ClubCategory,
                    // AppUserId = newClub.AppUserId,
                    Address = new Address
                    {
                        Street = newClub.Address.Street,
                        City = newClub.Address.City,
                        State = newClub.Address.State,
                    }
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(newClub);
        }
    }
}
