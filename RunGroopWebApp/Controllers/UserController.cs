using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Helpers;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.Utils;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPhotoService _photoService;

        public UserController(
            IUserRepository userRepository,
            IHttpContextAccessor contextAccessor,
            IPhotoService photoService
        )
        {
            _userRepository = userRepository;
            _contextAccessor = contextAccessor;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<AppUser> users = await _userRepository.GetAllUsers();
            List<UserViewModel> userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                UserViewModel userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    Mileage = user.Mileage,
                    ProfileImageUrl = string.IsNullOrEmpty(user.ProfileImageUrl)
                        ? AppConstants.DEFAULT_AVATAR_URL : user.ProfileImageUrl
                };
                userViewModels.Add(userViewModel);
            }

            return View(userViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            AppUser user = await _userRepository.GetUserById(id);
            if (user == null) return View("Error");

            UserDetailViewModel userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = string.IsNullOrEmpty(user.ProfileImageUrl)
                    ? AppConstants.DEFAULT_AVATAR_URL : user.ProfileImageUrl
            };

            return View(userDetailViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(string id)
        {
            AppUser user = await _userRepository.GetUserById(id);
            if (user == null) return View("Error");

            EditProfileViewModel editProfileViewModel = new EditProfileViewModel()
            {
                Id = user.Id,
                Pace = user.Pace,
                Mileage = user.Mileage,
                City = user.City,
                State = user.State,
                ProfileImageUrl = string.IsNullOrEmpty(user.ProfileImageUrl)
                    ? AppConstants.DEFAULT_AVATAR_URL : user.ProfileImageUrl
            };
            return View(editProfileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editProfileViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditProfile", editProfileViewModel);
            }

            AppUser user = await _userRepository.GetUserById(editProfileViewModel.Id);
            if (user == null) return View("Error");

            // Only update profile image
            if (editProfileViewModel.Image != null)
            {
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }

                var photoResult = _photoService.AddPhotoAsync(editProfileViewModel.Image);
                user.ProfileImageUrl = photoResult.Result.Url.ToString();
                _userRepository.Update(user);

                return RedirectToAction("EditProfile", "User", new { user.Id });
            }
            // Update all info except profile image
            else
            {
                user.Pace = editProfileViewModel.Pace;
                user.Mileage = editProfileViewModel.Mileage;
                user.City = editProfileViewModel.City;
                user.State = editProfileViewModel.State;
                _userRepository.Update(user);

                return RedirectToAction("Detail", "User", new { user.Id });
            }
        }
    }
}
