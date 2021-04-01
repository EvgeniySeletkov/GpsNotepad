using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace GpsNotepad.Validation
{
    static class Validator
    {
        public static bool HasFirstDigitalSymbol(string name)
        {
            var hasNumber = new Regex(@"^[0-9]");

            if (hasNumber.IsMatch(name))
            {
                return true;
            }
            return false;
        }

        public static bool HasValidEmail(string email)
        {
            try
            {
                var address = new MailAddress(email);
                return address.Address == email;
            }
            catch
            {

                return false;
            }
        }

        public static bool HasEqualPasswords(string password, string confirmPassword)
        {
            if (confirmPassword.Equals(password))
            {
                return true;
            }
            return false;
        }

        public static bool HasValidPassword(string password)
        {
            var hasNumber = new Regex("[0-9]+");
            var hasUpperChar = new Regex("[A-Z]+");
            var hasLowerChar = new Regex("[a-z]+");

            if (hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasLowerChar.IsMatch(password))
            {
                return true;
            }

            return false;
        }

        public static bool HasValidLength(string item, int minLength)
        {
            var hasCorrectLength = new Regex(@"^.{" + $"{minLength}" + ",16}$");

            if (hasCorrectLength.IsMatch(item))
            {
                return true;
            }
            return false;
        }
    }
}
