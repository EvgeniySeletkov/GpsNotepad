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
        //rename with verb and add -async
        Task<bool> HasEmail(string email);
        Task<bool> SignInAsync(string email, string password);
        //add -async
        Task SignInWithFacebook();

        //add -async
        Task CreateAccount(UserModel userModel);
    }
}
