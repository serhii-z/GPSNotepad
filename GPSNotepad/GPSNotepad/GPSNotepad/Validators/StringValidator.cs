using System.Text.RegularExpressions;

namespace GPSNotepad.Validators
{
    public static class StringValidator
    {
        public static bool IsFirstSimbolDigit(string item)
        {
            var hasNumber = new Regex(@"^[0-9]");

            if (hasNumber.IsMatch(item))
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
