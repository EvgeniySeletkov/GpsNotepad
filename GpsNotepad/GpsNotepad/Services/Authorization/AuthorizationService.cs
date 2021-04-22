using GpsNotepad.Models;
using GpsNotepad.Services.Repository;
using GpsNotepad.Services.Settings;
using GpsNotepad.Views;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GpsNotepad.Services.Authorization
{
    class AuthorizationService : IAuthorizationService
    {
        private IRepository _repository;
        private ISettingsManager _settingsManager;
        private INavigationService _navigationService;
        private IFacebookClient _facebookService = CrossFacebookClient.Current;

        public AuthorizationService(IRepository repository,
                                    ISettingsManager settingsManager,
                                    INavigationService navigationService)
        {
            _repository = repository;
            _settingsManager = settingsManager;
            _navigationService = navigationService;
        }
         
        private UserModel ConvertFacebookProfileModelToUserModel(FacebookProfileModel facebookProfileModel)
        {
            var userModel = new UserModel()
            {
                Email = facebookProfileModel.Email,
                Name = $"{facebookProfileModel.FirstName} {facebookProfileModel.LastName}"
            };

            return userModel;
        }

        private async Task<bool> AuthorizeWithFacebookAsync(FacebookProfileModel facebookProfileModel)
        {
            bool isAutorized = false;
            var userModel = ConvertFacebookProfileModelToUserModel(facebookProfileModel);
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
                await CreateAccount(userModel);
                isAutorized = true;
                _settingsManager.UserId = userModel.Id;
            }

            return isAutorized;

        }

        public bool IsAuthorized
        {
            get => _settingsManager.UserId != 0;
        }

        public async Task<bool> SignInAsync(string email, string password)
        {
            var users = await _repository.GetAllAsync<UserModel>();
            var user = users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                _settingsManager.UserId = user.Id;
            }

            return user != null;
        }

        public async Task SignInWithFacebook()
        {
            try
            {
                if (_facebookService.IsLoggedIn)
                {
                    _facebookService.Logout();
                }

                EventHandler<FBEventArgs<string>> userDataDelegate = null;

                userDataDelegate = async (object sender, FBEventArgs<string> e) =>
                {
                    if (e == null)
                    {
                        return;
                    }

                    switch (e.Status)
                    {
                        case FacebookActionStatus.Completed:
                            var facebookProfileModel = await Task.Run(() => JsonConvert.DeserializeObject<FacebookProfileModel>(e.Data));
                            bool isAutorized = await AuthorizeWithFacebookAsync(facebookProfileModel);
                            if (isAutorized)
                            {
                                await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainMapTabbedPage)}");
                            }
                            break;
                        case FacebookActionStatus.Canceled:
                            break;
                        case FacebookActionStatus.Error:
                            break;
                        case FacebookActionStatus.Unauthorized:
                            break;
                    }

                    _facebookService.OnUserData -= userDataDelegate;
                };

                _facebookService.OnUserData += userDataDelegate;

                string[] fbRequestFields = { "email", "first_name", "gender", "last_name" };
                string[] fbPermisions = { "email" };
                await _facebookService.RequestUserDataAsync(fbRequestFields, fbPermisions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public async Task CreateAccount(UserModel userModel)
        {
            await _repository.InsertAsync(userModel);
        }

        public async Task<bool> HasEmail(string email)
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
    }
}
