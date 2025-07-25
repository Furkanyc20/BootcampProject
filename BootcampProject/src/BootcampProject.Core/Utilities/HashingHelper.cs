using System;
using System.Security.Cryptography;
using System.Text;

namespace BootcampProject.Core.Utilities
{
    public static class HashingHelper
    {
        /// <summary>
        /// Creates a password hash and salt using HMACSHA512
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <param name="passwordHash">The generated password hash</param>
        /// <param name="passwordSalt">The generated password salt</param>
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty or whitespace only string.", nameof(password));

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Verifies a password against the stored hash and salt
        /// </summary>
        /// <param name="password">The password to verify</param>
        /// <param name="passwordHash">The stored password hash</param>
        /// <param name="passwordSalt">The stored password salt</param>
        /// <returns>True if password is correct, false otherwise</returns>
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty or whitespace only string.", nameof(password));

            if (passwordHash == null)
                throw new ArgumentNullException(nameof(passwordHash));

            if (passwordSalt == null)
                throw new ArgumentNullException(nameof(passwordSalt));

            if (passwordHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(passwordHash));

            if (passwordSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(passwordSalt));

            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                
                // Compare the computed hash with the stored hash
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Creates a password hash using a simple SHA256 approach with salt
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <param name="salt">The salt to use</param>
        /// <returns>Base64 encoded hash</returns>
        public static string CreateSHA256Hash(string password, string salt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty or whitespace only string.", nameof(password));

            if (string.IsNullOrWhiteSpace(salt))
                throw new ArgumentException("Salt cannot be empty or whitespace only string.", nameof(salt));

            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        /// <summary>
        /// Verifies a password against SHA256 hash with salt
        /// </summary>
        /// <param name="password">The password to verify</param>
        /// <param name="hash">The stored hash</param>
        /// <param name="salt">The salt used</param>
        /// <returns>True if password is correct, false otherwise</returns>
        public static bool VerifySHA256Hash(string password, string hash, string salt)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (string.IsNullOrWhiteSpace(hash))
                return false;

            if (string.IsNullOrWhiteSpace(salt))
                return false;

            var computedHash = CreateSHA256Hash(password, salt);
            return computedHash == hash;
        }

        /// <summary>
        /// Generates a cryptographically secure random salt
        /// </summary>
        /// <param name="size">Size of the salt in bytes (default: 32)</param>
        /// <returns>Base64 encoded salt</returns>
        public static string GenerateSalt(int size = 32)
        {
            if (size <= 0)
                throw new ArgumentException("Salt size must be greater than 0.", nameof(size));

            var saltBytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Generates a random password of specified length
        /// </summary>
        /// <param name="length">Length of the password (default: 12)</param>
        /// <param name="includeSpecialChars">Include special characters (default: true)</param>
        /// <returns>Generated password</returns>
        public static string GenerateRandomPassword(int length = 12, bool includeSpecialChars = true)
        {
            if (length <= 0)
                throw new ArgumentException("Password length must be greater than 0.", nameof(length));

            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

            var chars = lowercase + uppercase + digits;
            if (includeSpecialChars)
                chars += specialChars;

            var password = new StringBuilder();
            using (var rng = RandomNumberGenerator.Create())
            {
                var buffer = new byte[sizeof(uint)];
                for (int i = 0; i < length; i++)
                {
                    rng.GetBytes(buffer);
                    var randomIndex = BitConverter.ToUInt32(buffer, 0) % chars.Length;
                    password.Append(chars[(int)randomIndex]);
                }
            }

            return password.ToString();
        }
    }
}