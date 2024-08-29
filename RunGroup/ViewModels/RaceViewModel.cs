using RunGroup.Data.Enum;

namespace RunGroup.ViewModels
{
    public class RaceViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public RaceCategory RaceCategory { get; set; }
        public int? AddressId { get; set; }
        public string? AddressStreet { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressState { get; set; }
    }
}
