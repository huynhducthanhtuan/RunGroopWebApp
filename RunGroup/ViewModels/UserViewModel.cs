using RunGroup.Utils;

namespace RunGroup.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? ProfileImageUrl { get; set; }

        public UserViewModel ()
        {
            ProfileImageUrl = AppConstants.DEFAULT_AVATAR_URL;
        }
    }
}
