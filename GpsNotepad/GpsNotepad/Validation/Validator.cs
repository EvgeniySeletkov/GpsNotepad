using System.Text.RegularExpressions;

namespace GpsNotepad.Validation
{
    static class Validator
    {

        public static bool HasValidName(string name)
        {
            bool isName = false;
            var nameRegex = new Regex(@"^[A-Za-z][A-Za-z\d]{3,15}$");

            if (nameRegex.IsMatch(name))
            {
                isName = true;
            }
            return isName;
        }

        public static bool HasValidEmail(string email)
        {
            bool isEmail = false;
            var emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (emailRegex.IsMatch(email))
            {
                isEmail = true;
            }
            return isEmail;
        }

        public static bool HasValidPassword(string password)
        {
            bool isPassword = false;
            var passwordRegex = new Regex(@"^[A-Z](?=.*[a-z])(?=.*\d)[a-zA-Z\d]{5,15}$");

            if (passwordRegex.IsMatch(password))
            {
                isPassword = true;
            }
            return isPassword;
        }

        public static bool HasEqualPasswords(string password, string confirmPassword)
        {
            bool arePasswordsEqual = false;
            if (confirmPassword.Equals(password))
            {
                arePasswordsEqual = true;
            }
            return arePasswordsEqual;
        }

    }
}
