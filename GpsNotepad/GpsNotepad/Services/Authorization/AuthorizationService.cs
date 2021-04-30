using GpsNotepad.Extensions;
using GpsNotepad.Models;
using GpsNotepad.Services.Repository;
using GpsNotepad.Services.Settings;
using Newtonsoft.Json.Linq;
using Plugin.FacebookClient;
using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;

namespace GpsNotepad.Services.Authorization
{
    class AuthorizationService : IAuthorizationService
    {
        private readonly IRepository _repository;
        private readonly ISettingsManager _settingsManager;
        private readonly INavigationService _navigationService;
        private readonly IFacebookClient _facebookService;
        
        private string[] _fbPermisions = { Constants.FACEBOOK_EMAIL_KEY };

        public AuthorizationService(IRepository repository,
                                    ISettingsManager settingsManager,
                                    INavigationService navigationService)
        {
            _repository = repository;
            _settingsManager = settingsManager;
            _navigationService = navigationService;
            _facebookService = CrossFacebookClient.Current;
        }

        public bool IsAuthorized
        {
            get => _settingsManager.UserId != 0;
        }

        public Task CreateAccountAsync(UserModel userModel)
        {
            return _repository.InsertAsync(userModel);
        }

        public async Task<bool> HasEmailAsync(string email)
        {
            bool result = false;
            var users = await _repository.GetAllAsync<UserModel>();
            var user = users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                result = true;
            }

            return result;
        }

        public async Task<bool> LogInAsync(string email, string password)
        {
            var users = await _repository.GetAllAsync<UserModel>();
            var user = users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                _settingsManager.UserId = user.Id;
            }

            return user != null;
        }

        private async Task<FacebookProfileModel> LoadFacebookDataAsync()
        {
            string[] fbRequestFields = new string[]
            { 
                Constants.FACEBOOK_EMAIL_KEY, 
                Constants.FACEBOOK_FIRST_NAME_KEY, 
                Constants.FACEBOOK_LAST_NAME_KEY
            };

            var jsonData = await _facebookService.RequestUserDataAsync(fbRequestFields, _fbPermisions);
            var data = JObject.Parse(jsonData.Data);

            var facebookProfile = new FacebookProfileModel()
            {
                FirstName = data[Constants.FACEBOOK_FIRST_NAME_KEY].ToString(),
                LastName = data[Constants.FACEBOOK_LAST_NAME_KEY].ToString(),
                Email = data[Constants.FACEBOOK_EMAIL_KEY].ToString()
            };

            return facebookProfile;
        }

        public Task<FacebookResponse<bool>> LogInWithFacebookAsync()
        {
            return _facebookService.LoginAsync(_fbPermisions);
        }

        public async Task<bool> AuthorizeWithFacebookAsync()
        {
            bool isAutorized = false;
            var facebookProfile = await LoadFacebookDataAsync();
            var userModel = facebookProfile.ToUser();
            var users = await _repository.GetAllAsync<UserModel>();
            var user = users.FirstOrDefault(u => u.Email == userModel.Email);

            if (user != null)
            {
                if (string.IsNullOrWhiteSpace(user.Password))
                {
                    isAutorized = true;
                    _settingsManager.UserId = user.Id;
                }
                else
                {
                    isAutorized = false;
                }
            }
            else
            {
                await CreateAccountAsync(userModel);
                isAutorized = true;
                _settingsManager.UserId = userModel.Id;
            }

            return isAutorized;

        }

        public void LogOut()
        {
            _settingsManager.ClearSettings();
        }
    }
}
