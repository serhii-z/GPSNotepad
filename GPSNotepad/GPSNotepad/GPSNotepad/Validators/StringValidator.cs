using System.Text.RegularExpressions;

namespace GPSNotepad.Validators
{
    public static class StringValidator
    {
        public static bool IsValidName(string name)
        {
            var hasSequence = new Regex(@"[A-Z][a-z]+\s[A-Z][a-z]+");

            if (hasSequence.IsMatch(name))
            {
                return true;
            }

            return false;
        }
        public static bool IsValidEmail(string email)
        {
            var hasSequence = new Regex(@"[a-z0-9._%+-]+@[a-z]+.[a-z]{2,4}");

            if (hasSequence.IsMatch(email))
            {
                return true;
            }

            return false;
        }

        public static bool IsPasswordsEqual(string password, string confirm)
        {
            if (password.Equals(confirm))
            {
                return true;
            }

            return false;
        }

        public static bool IsQuantityCorrect(string item, int minLength)
        {
            var pattern = @"^.{" + $"{minLength}" + ",16}$";
            var hasSequence = new Regex(pattern);

            if (hasSequence.IsMatch(item))
            {
                return true;
            }

            return false;
        }

        public static bool IsAvailability(string item)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");

            if (hasNumber.IsMatch(item) && hasUpperChar.IsMatch(item) && hasLowerChar.IsMatch(item))
            {
                return true;
            }

            return false;
        }
    }
}
