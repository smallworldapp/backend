using System;
using System.Security.Cryptography;

namespace SmallWorld.Database
{
    public class Token
    {
        private static readonly RandomNumberGenerator RNG = RandomNumberGenerator.Create();

        public static Guid Generate()
        {
            var bytes = new byte[16];
            RNG.GetBytes(bytes);
            return new Guid(bytes);
        }
    }
}