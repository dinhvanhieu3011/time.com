using System;
using System.Text;
using BASE.Entity.IdentityAccess;
using Microsoft.AspNetCore.Identity;

namespace BMBSOFT.GIS.ApiGateway.Configuration
{
    public class Password
    {
        private readonly UserManager<User> _userManager;

        public Password(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public string GeneratePassword()
        {
            var options = _userManager.Options.Password;

            var length = options.RequiredLength;

            var nonAlphanumeric = options.RequireNonAlphanumeric;
            var digit = options.RequireDigit;
            var lowercase = options.RequireLowercase;
            var uppercase = options.RequireUppercase;

            var password = new StringBuilder();
            var random = new Random();

            while (password.Length < length)
            {
                var c = (char)random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            return password.ToString();
        }
    }
}
