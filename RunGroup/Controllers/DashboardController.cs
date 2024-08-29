using Microsoft.AspNetCore.Mvc;
using RunGroup.Interfaces;
using RunGroup.Models;
using RunGroup.ViewModels;

namespace RunGroup.Controllers
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
            IEnumerable<Club> clubs = await _dashboardRepository.GetAllUserClubs();
            IEnumerable<Race> races = await _dashboardRepository.GetAllUserRaces();

            DashboardViewModel dashboardInfo = new DashboardViewModel()
            {
                Clubs = clubs,
                Races = races
            };
            return View(dashboardInfo);
        }
    }
}
