using PlanningBook.Identity.Application.Helpers.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PlanningBook.Identity.Application.Helpers
{
    public sealed class PasswordHasher : IPasswordHasher
    {
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const int saltSize = 16;
        private const int hashSize = 32;
        private const int iterations = 500000;
        private static readonly HashAlgorithmName algorithm = HashAlgorithmName.SHA512;

        public string Hash(string passwordPlainText)
        {
            byte[] salt = GenerateRandomSalt(saltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(passwordPlainText, salt, iterations, algorithm, hashSize);

            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }

        public bool Verify(string passwordPlainText, string passwordHash)
        {
            string[] parts = passwordHash.Split('-');
            byte[] hash = Convert.FromHexString(parts[0]);
            byte[] salt = Convert.FromHexString(parts[1]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(passwordPlainText, salt, iterations, algorithm, hashSize);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }

        #region Private methods
        private byte[] GenerateRandomSalt(int saltSize)
        {
            var rawSalt = new StringBuilder(saltSize);
            byte[] randomBytes = new byte[1];

            using (var rng = RandomNumberGenerator.Create())
            {
                while (rawSalt.Length < saltSize)
                {
                    rng.GetBytes(randomBytes);

                    // Get random index from chars
                    int index = randomBytes[0] % chars.Length;

                    rawSalt.Append(chars[index]);
                }
            }

            byte[] salt = Encoding.UTF8.GetBytes(rawSalt.ToString());

            return salt;
        }
        #endregion Private methods
    }
}
