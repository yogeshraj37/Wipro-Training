// ==========================================
// Attributes/ValidIsbnAttribute.cs
// ==========================================
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BookstoreApp.Attributes
{
    /// <summary>
    /// Validates ISBN-10 or ISBN-13 format.
    /// ISBN-10: 10 digits (last can be X), optional hyphens.
    /// ISBN-13: 13 digits, optional hyphens.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ValidIsbnAttribute : ValidationAttribute
    {
        // Strips hyphens and spaces, then checks digit count and checksum
        private static readonly Regex Isbn10Pattern = new(@"^\d{9}[\dX]$", RegexOptions.Compiled);
        private static readonly Regex Isbn13Pattern = new(@"^\d{13}$", RegexOptions.Compiled);

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
                return ValidationResult.Success; // Let [Required] handle null/empty

            string isbn = value.ToString()!.Replace("-", "").Replace(" ", "").Trim().ToUpper();

            if (Isbn10Pattern.IsMatch(isbn))
            {
                if (IsValidIsbn10(isbn))
                    return ValidationResult.Success;
                return new ValidationResult("ISBN-10 checksum is invalid.");
            }

            if (Isbn13Pattern.IsMatch(isbn))
            {
                if (IsValidIsbn13(isbn))
                    return ValidationResult.Success;
                return new ValidationResult("ISBN-13 checksum is invalid.");
            }

            return new ValidationResult(ErrorMessage ?? "ISBN must be a valid ISBN-10 or ISBN-13.");
        }

        private static bool IsValidIsbn10(string isbn)
        {
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += (isbn[i] - '0') * (10 - i);

            char lastChar = isbn[9];
            sum += lastChar == 'X' ? 10 : (lastChar - '0');
            return sum % 11 == 0;
        }

        private static bool IsValidIsbn13(string isbn)
        {
            int sum = 0;
            for (int i = 0; i < 12; i++)
                sum += (isbn[i] - '0') * (i % 2 == 0 ? 1 : 3);

            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == (isbn[12] - '0');
        }
    }
}

// ==========================================
// Attributes/ValidBookPriceAttribute.cs
// ==========================================
using System.ComponentModel.DataAnnotations;

namespace BookstoreApp.Attributes
{
    /// <summary>
    /// Validates that the book price falls within an acceptable range.
    /// Prevents zero or negative prices and unreasonably large values.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ValidBookPriceAttribute : ValidationAttribute
    {
        public double Min { get; set; } = 0.01;
        public double Max { get; set; } = 99999.99;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            if (!decimal.TryParse(value.ToString(), out decimal price))
                return new ValidationResult("Price must be a valid decimal number.");

            if (price < (decimal)Min)
                return new ValidationResult($"Price must be at least {Min:C2}.");

            if (price > (decimal)Max)
                return new ValidationResult($"Price cannot exceed {Max:C2}.");

            // Validate no more than 2 decimal places
            if (decimal.Round(price, 2) != price)
                return new ValidationResult("Price must not have more than 2 decimal places.");

            return ValidationResult.Success;
        }
    }
}
