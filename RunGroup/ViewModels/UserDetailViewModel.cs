using RunGroup.Utils;

namespace RunGroup.ViewModels
{
    public class UserDetailViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        public string? ProfileImageUrl { get; set; }
        public int? AddressId { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        public UserDetailViewModel()
        {
            ProfileImageUrl = AppConstants.DEFAULT_AVATAR_URL;
        }
    }
}
