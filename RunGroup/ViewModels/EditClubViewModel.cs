using RunGroup.Data.Enum;
using RunGroup.Models;

namespace RunGroup.ViewModels
{
    public class EditClubViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ClubCategory ClubCategory { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public IFormFile Image { get; set; }
        public string? ImageUrl { get; set; }
    }
}