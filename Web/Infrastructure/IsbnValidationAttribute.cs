using System.ComponentModel.DataAnnotations;
using Utils;

namespace Web.Infrastructure
{
    public class IsbnAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string val = (string) value;

            if (!string.IsNullOrEmpty(val))
            {
                val = val.Replace("-", "");

                if (val.Length == 10)
                {
                    return IsValidIsbn10(val);
                }

                if (val.Length == 13)
                {
                    return IsValidIsbn13(val);
                }

                return false;
            }

            return false;
        }


        private static bool IsValidIsbn10(string isbn)
        {
            int sum = 0;
            int val;

            for (int i = 0; i < 9; i++)
            {
                char c = isbn[i];
                if (c.TryParse(out val))
                {
                    sum += (i + 1) * val;
                }
                else
                {
                    return false;
                }
            }

            int remainder = sum % 11;
            char lastCharacter = isbn[isbn.Length - 1];

            if (lastCharacter == 'X')
            {
                return remainder == 10;
            }

            if (lastCharacter.TryParse(out val))
            {
                return remainder == val;
            }

            return false;
        }

        private static bool IsValidIsbn13(string isbn)
        {
            int sum = 0;
            int val;

            for (int i = 0; i < 12; i++)
            {
                char c = isbn[i];
                if (c.TryParse(out val))
                {
                    sum += (i % 2 == 1 ? 3 : 1)*val;
                }
                else
                {
                    return false;
                }
            }

            int remainder = sum % 10;
            int checkDigit = 10 - remainder;
            if (checkDigit == 10) checkDigit = 0;

            char lastCharacter = isbn[isbn.Length - 1];

            if (lastCharacter.TryParse(out val))
            {
                return checkDigit == val;
            }

            return false;
        }
    }
}