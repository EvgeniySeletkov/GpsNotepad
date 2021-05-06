using GpsNotepad.Models;
using Plugin.FacebookClient;
using System.Threading.Tasks;

namespace GpsNotepad.Services.Authorization
{
    interface IAuthorizationService
    {

        bool IsAuthorized { get; }

        Task CreateAccountAsync(UserModel userModel);

        Task<bool> HasEmailAsync(string email);

        Task<bool> LogInAsync(string email, string password);

        Task<FacebookResponse<bool>> LogInWithFacebookAsync();

        Task<bool> AuthorizeWithFacebookAsync();

        void LogOut();
        
    }
}
