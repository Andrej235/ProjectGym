using System.Security.Cryptography;

namespace ProjectGym.Services
{
    public class HashingService
    {
        public static byte[] HashPassword(string password, byte[] salt) => SHA256.HashData(CombineBytes(salt, System.Text.Encoding.UTF8.GetBytes(password)));

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            RandomNumberGenerator.Create().GetBytes(salt);
            return salt;
        }

        private static byte[] CombineBytes(byte[] first, byte[] second)
        {
            byte[] combined = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, combined, 0, first.Length);
            Buffer.BlockCopy(second, 0, combined, first.Length, second.Length);
            return combined;
        }
    }
}
