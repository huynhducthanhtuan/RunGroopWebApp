using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Club> clubs = await _dashboardRepository.GetAllUserClubs();
            List<Race> races = await _dashboardRepository.GetAllUserRaces();
            var dashboardInfo = new DashboardViewModel()
            {
                Clubs = clubs,
                Races = races
            };
            return View(dashboardInfo);
        }
    }
}
