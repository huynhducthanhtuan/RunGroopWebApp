using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroup.Interfaces;
using RunGroup.ViewModels;

namespace RunGroup.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;

        public UserController(IUserRepository userRepository, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<UserViewModel> users = await _userRepository.GetAllUsers();
            if (users == null) return View("Error");
            else return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            UserDetailViewModel user = await _userRepository.GetUserById(id);
            if (user == null) return View("Error");
            else return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(string id)
        {
            UserDetailViewModel user = await _userRepository.GetUserById(id);
            if (user == null) return View("Error");

            EditProfileViewModel editProfileViewModel = new EditProfileViewModel()
            {
                Id = user.Id,
                Pace = user.Pace,
                Mileage = user.Mileage,
                Street = user.Street,
                City = user.City,
                State = user.State,
                ProfileImageUrl = user.ProfileImageUrl
            };
            return View(editProfileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(string id, EditProfileViewModel editProfileViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditProfile", editProfileViewModel);
            }

            UserDetailViewModel user = await _userRepository.GetUserById(id);
            if (user == null) return View("Error");

            // Only update profile image
            if (editProfileViewModel.Image != null)
            {
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }

                ImageUploadResult photoResult = await _photoService.AddPhotoAsync(editProfileViewModel.Image);
                string profileImageUrl = photoResult.Url.ToString();

                await _userRepository.UpdateProfileImageUrl(user.Id, profileImageUrl);
                return RedirectToAction("EditProfile", "User", new { user.Id });
            }
            // Update rest info
            else
            {
                user.Pace = editProfileViewModel.Pace;
                user.Mileage = editProfileViewModel.Mileage;
                user.Street = editProfileViewModel.Street;
                user.City = editProfileViewModel.City;
                user.State = editProfileViewModel.State;

                await _userRepository.Update(user);
                return RedirectToAction("Detail", "User", new { user.Id });
            }
        }
    }
}
