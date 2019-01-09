using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SmallWorld.Database.Entities;

namespace SmallWorld.Database.Tests.Fakes
{
    public class CryptoProvider
    {
        private const int saltSize = 74;
        private readonly HashAlgorithm hashAlgorithm = SHA256.Create();
        private readonly RandomNumberGenerator random = RandomNumberGenerator.Create();

        public string CreatePassword()
        {
            var legal = "abcdefghijklmnopqrstuvwxyz";
            legal = legal + legal.ToUpper() + "0123456789";

            var build = new StringBuilder();
            var rand = new Random();
            for (var i = 0; i < 10; i++)
            {
                var c = legal[rand.Next(legal.Length)];
                build.Append(c);
            }
            var pass = build.ToString();

            return pass;
        }

        public byte[] GenerateSalt()
        {
            var bytes = new byte[saltSize];
            random.GetBytes(bytes);
            return bytes;
        }

        public byte[] GenerateHash(byte[] salt, string secret)
        {
            var raw = Encoding.UTF8.GetBytes(secret);
            var bytes = new byte[salt.Length + raw.Length];
            Array.Copy(salt, bytes, salt.Length);
            Array.Copy(raw, 0, bytes, salt.Length, raw.Length);
            return hashAlgorithm.ComputeHash(bytes);
        }

        public bool Authenticate(Account acc, string credsPassword)
        {
            return GenerateHash(acc.Credentials.Salt, credsPassword).SequenceEqual(acc.Credentials.Hash);
        }
    }
}
