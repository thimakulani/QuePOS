using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using QuePOS.API.Data;
using QuePOS.API.Interfaces;
using QuePOS.API.Models;
using System.Security.Claims;
using System.Text;

namespace QuePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreUserController : ControllerBase
    {
        private readonly IEmailService _mailService;
        private readonly IRepository<StoreUser> _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly POSDbContext dbContext;

        public StoreUserController(IEmailService mailService, IRepository<StoreUser> repository, UserManager<ApplicationUser> userManager, POSDbContext dbContext)
        {
            _mailService = mailService;
            _repository = repository;
            _userManager = userManager;
            this.dbContext = dbContext;
        }
        [HttpPost]
        public async Task<IActionResult> Add(StoreUser storeUser)
        {
            var password = GeneratePassword();
            string body = $"Your password for QUE POS app is: {password}. Please keep it confidential and do not share it with anyone. If you did not request this, please contact support.";
            ApplicationUser applicationUser = new()
            {
                PhoneNumber = storeUser.PhoneNumber,
                Email = storeUser.Email.Trim(),
            };
            var results = await _userManager.CreateAsync(applicationUser, password);
            if (results.Succeeded)
            {
                var appUser = await _userManager.FindByEmailAsync(storeUser.Email);
                var user = await _repository.Add(storeUser);
                await _userManager.AddToRoleAsync(appUser, "Store Employee");
                await _mailService.SendAsync(storeUser.Email, "Password", body);
                return Ok(user);
            }
            var errors = string.Join("\n", results.Errors.Select(e => e.Description));
            return BadRequest(errors);
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var new_store = await _repository.GetList();
            return Ok(new_store);
        }
        [HttpGet("users")]
        public async Task<IActionResult> AllStoreUsers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var store = dbContext.Stores.Where(x => x.StoreUserId == userId).FirstOrDefault();
            var new_store = await _repository.GetWhere(x => x.StoreID == store.Id);
            return Ok(new_store);
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
            const string nonAlphanumeric = "!@#$%&*()-_=+[]{}|<>?";
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
            return new string([.. input.ToCharArray().OrderBy(_ => random.Next())]);
        }
    }


}
