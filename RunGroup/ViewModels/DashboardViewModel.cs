using RunGroup.Models;

namespace RunGroup.ViewModels
{
    public class DashboardViewModel
    {
        public IEnumerable<Club> Clubs { get; set; }
        public IEnumerable<Race> Races { get; set; }
    }
}
