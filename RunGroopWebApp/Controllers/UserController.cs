using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<UserViewModel> users = await _userRepository.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            UserDetailViewModel user = await _userRepository.GetUserById(id);
            return View(user);
        }
    }
}
