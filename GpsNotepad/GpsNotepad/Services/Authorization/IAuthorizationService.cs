using GpsNotepad.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GpsNotepad.Services.Authorization
{
    interface IAuthorizationService
    {
        bool IsAuthorized { get; }
        Task<bool> HasEmail(string email);
        Task<bool> SignInAsync(string email, string password);
        Task SignInWithFacebook();
        Task CreateAccount(UserModel userModel);
    }
}
