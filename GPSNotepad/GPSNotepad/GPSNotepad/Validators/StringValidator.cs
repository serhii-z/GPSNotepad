using System.Text.RegularExpressions;

namespace GPSNotepad.Validators
{
    public static class StringValidator
    {
        public static bool CheckName(string name)
        {
            var hasSequence = new Regex(@"[A-Z][a-z]+\s[A-Z][a-z]+");

            return hasSequence.IsMatch(name);
        }
        public static bool CheckLogin(string email)
        {
            var hasSequence = new Regex(@"[a-z0-9._%+-]+@[a-z]+.[a-z]{2,4}");

            return hasSequence.IsMatch(email);
        }

        public static bool CheckPasswordEquality(string password, string confirm)
        {
            return password.Equals(confirm);
        }

        public static bool CheckQuantity(string item, int minLength)
        {
            var pattern = @"^.{" + $"{minLength}" + ",16}$";
            var hasSequence = new Regex(pattern);

            return hasSequence.IsMatch(item);
        }

        public static bool CheckPresence(string item)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");

            return hasNumber.IsMatch(item) && hasUpperChar.IsMatch(item) && hasLowerChar.IsMatch(item);
        }
    }
}
