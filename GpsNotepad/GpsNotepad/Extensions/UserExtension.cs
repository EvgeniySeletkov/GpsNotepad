using GpsNotepad.Models;

namespace GpsNotepad.Extensions
{
    static class UserExtension
    {
        public static UserModel ToUser(this FacebookProfileModel facebookProfile)
        {
            var user = new UserModel()
            {
                Email = facebookProfile.Email,
                Name = $"{facebookProfile.FirstName} {facebookProfile.LastName}"
            };

            return user;
        }
    }
}
