using System;
using System.Security.Cryptography;
using System.Text;

namespace NET.Identity.Helpers
{
    /// <summary>
    /// Şifre hash'leme ve doğrulama yardımcı sınıfı
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>
        /// Şifreyi hash'ler
        /// </summary>
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        /// <summary>
        /// Şifre doğrulama
        /// </summary>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hashedPassword;
        }

        /// <summary>
        /// Şifre sıfırlama tokeni oluşturma
        /// </summary>
        public static string GenerateResetToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var tokenBytes = new byte[32];
            rng.GetBytes(tokenBytes);
            return Convert.ToBase64String(tokenBytes);
        }
    }
}