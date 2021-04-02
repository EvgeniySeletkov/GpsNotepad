using GpsNotepad.Models;
using GpsNotepad.Services.Repository;
using GpsNotepad.Services.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpsNotepad.Services.Authorization
{
    class AuthorizationService : IAuthorizationService
    {
        private IRepository repository;
        private ISettingsManager settingsManager;

        public AuthorizationService(IRepository repository,
                                    ISettingsManager settingsManager)
        {
            this.repository = repository;
            this.settingsManager = settingsManager;
        }

        public bool IsAuthorized
        {
            get => settingsManager.UserId != 0;
        }

        public async Task<bool> SignInAsync(string email, string password)
        {
            var users = await repository.GetAllAsync<UserModel>();
            var user = users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                settingsManager.UserId = user.Id;
            }

            return user != null;
        }

        public async Task<bool> SignUp(UserModel userModel)
        {
            var users = await repository.GetAllAsync<UserModel>();
            var user = users.FirstOrDefault(u => u.Email == userModel.Email);

            if (user == null)
            {
                await repository.InsertAsync(userModel);
                return true;
            }

            return false;
        }
    }
}
