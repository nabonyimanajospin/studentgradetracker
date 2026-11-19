using System;
using System.Security.Cryptography;

namespace StudentGradeTracker.Models
{
    // MODEL: turns a password into a salted hash, so the database never stores the real password.
    // Stored format:  pbkdf2$<iterations>$<salt>$<hash>
    public static class PasswordHasher
    {
        const string Prefix     = "pbkdf2";
        const int    Iterations = 100000;
        const int    SaltBytes  = 16;
        const int    HashBytes  = 32;

        public static string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltBytes);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashBytes);
            return Prefix + "$" + Iterations + "$" + Convert.ToBase64String(salt) + "$" + Convert.ToBase64String(hash);
        }

        public static bool IsHashed(string stored)
        {
            return stored != null && stored.StartsWith(Prefix + "$");
        }

        public static bool Verify(string password, string stored)
        {
            if (!IsHashed(stored))
                return false;

            string[] parts = stored.Split('$');
            int    iterations = int.Parse(parts[1]);
            byte[] salt       = Convert.FromBase64String(parts[2]);
            byte[] expected   = Convert.FromBase64String(parts[3]);
            byte[] actual     = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);

            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
    }
}
