using System;
using System.Text;
using System.Security.Cryptography;

namespace Images.Data
{
    public static class PasswordManager
    {
        public static string HashPassword(string password, string salt)
        {
            SHA256Managed crypt = new SHA256Managed();
            string combinedString = password + salt;
            byte[] combined = Encoding.Unicode.GetBytes(combinedString);

            byte[] hash = crypt.ComputeHash(combined);
            return Convert.ToBase64String(hash);
        }

        public static string GenerateSalt()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[10];
            provider.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public static bool isMatch (string hashedPassword, string passwordAttempt, string salt)
        {
           string attempt = HashPassword(passwordAttempt, salt);
            return hashedPassword == attempt;
        }
    }
}
