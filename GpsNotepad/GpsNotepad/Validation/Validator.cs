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
            bool isFirstSymbolDigit = false;
            var hasNumber = new Regex(@"^[0-9]");

            if (hasNumber.IsMatch(name))
            {
                isFirstSymbolDigit = true;
            }
            return isFirstSymbolDigit;
        }

        public static bool HasValidEmail(string email)
        {
            bool isEmail;
            try
            {
                var address = new MailAddress(email);
                isEmail = address.Address == email;
            }
            catch
            {
                isEmail = false;
            }
            return isEmail;
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

        public static bool HasValidPassword(string password)
        {
            bool isPasswordValid = false;
            var hasNumber = new Regex("[0-9]+");
            var hasUpperChar = new Regex("[A-Z]+");
            var hasLowerChar = new Regex("[a-z]+");

            if (hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasLowerChar.IsMatch(password))
            {
                isPasswordValid = true;
            }

            return isPasswordValid;
        }

        public static bool HasValidLength(string item, int minLength)
        {
            bool isLengthValid = false;
            var hasCorrectLength = new Regex(@"^.{" + $"{minLength}" + ",16}$");

            if (hasCorrectLength.IsMatch(item))
            {
                isLengthValid = true;
            }
            return isLengthValid;
        }
    }
}
