using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RunGroup.Helpers;
using RunGroup.Interfaces;
using RunGroup.Models;
using RunGroup.ViewModels;
using System.Diagnostics;
using System.Globalization;
using System.Net;

namespace RunGroup.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClubRepository _clubRepository;

        public HomeController(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IPInfo ipInfo = new IPInfo();
            HomeViewModel homeViewModel = new HomeViewModel();

            try
            {
                // Get clubs in your city using IPInfo API
                string url = "https://ipinfo.io?token=70dcbe0c31efe5";
                var info = new WebClient().DownloadString(url);

                // Convert JSON to IPInfo object
                ipInfo = JsonConvert.DeserializeObject<IPInfo>(info);

                RegionInfo regionInfo = new RegionInfo(ipInfo.Country);
                ipInfo.Country = regionInfo.EnglishName;

                homeViewModel.City = ipInfo.City;
                homeViewModel.State = ipInfo.Region;

                if (homeViewModel.City != null)
                {
                    IEnumerable<Club> clubs = await _clubRepository.GetClubsByCity(homeViewModel.City);
                    if (clubs.Count() > 0)
                    {
                        homeViewModel.Clubs = clubs;
                    }
                }
                return View(homeViewModel);
            }
            catch (Exception ex)
            {
                homeViewModel.Clubs = null;
            }

            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}