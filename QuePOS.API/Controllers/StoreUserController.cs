using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuePOS.API.Models;
using System.Text;

namespace QuePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreUserController : ControllerBase
    {


        public async Task<IActionResult> Add(StoreUser storeUser)
        {
            var password = GeneratePassword();
            string body = $"Your password for EasyCleaar mobile app is: {password}. Please keep it confidential and do not share it with anyone. If you did not request this, please contact support at support@easycleaar.co.za.";
        
        }

        public static string GeneratePassword(int length = 8, bool requireDigit = true, bool requireLowercase = true,
                                      bool requireUppercase = true, bool requireNonAlphanumeric = true)
        {
            if (length < 8)
                throw new ArgumentException("Password length must be at least 8.", nameof(length));

            // Character pools
            const string digits = "0123456789";
            const string lowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
            const string upperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string nonAlphanumeric = "!@#$%^&*()-_=+[]{}|;:,.<>?";
            const string allCharacters = digits + lowerCaseLetters + upperCaseLetters + nonAlphanumeric;

            var passwordBuilder = new StringBuilder();

            // Ensure at least one character of each required type
            if (requireDigit)
                passwordBuilder.Append(GetRandomChar(digits));
            if (requireLowercase)
                passwordBuilder.Append(GetRandomChar(lowerCaseLetters));
            if (requireUppercase)
                passwordBuilder.Append(GetRandomChar(upperCaseLetters));
            if (requireNonAlphanumeric)
                passwordBuilder.Append(GetRandomChar(nonAlphanumeric));

            // Fill the rest of the password with random characters from the allowed pool
            var remainingLength = length - passwordBuilder.Length;
            if (remainingLength > 0)
            {
                passwordBuilder.Append(GenerateRandomString(allCharacters, remainingLength));
            }

            // Shuffle the characters to ensure randomness
            return Shuffle(passwordBuilder.ToString());
        }

        private static char GetRandomChar(string input)
        {
            var random = new Random();
            return input[random.Next(input.Length)];
        }

        private static string GenerateRandomString(string input, int length)
        {
            var random = new Random();
            return new string(Enumerable.Repeat(input, length)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());
        }

        private static string Shuffle(string input)
        {
            var random = new Random();
            return new string(input.ToCharArray()
                                   .OrderBy(_ => random.Next())
                                   .ToArray());
        }
    }


}
