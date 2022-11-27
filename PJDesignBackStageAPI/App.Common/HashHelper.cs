using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common
{
    public class HashHelper
    {
        private static string salt = "-r4T8aMmAWdmzo4RZRyXtuD@cyrewk";
        public static string GetPbkdf2Value(string source)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: source,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000,
                numBytesRequested: 16));

            return hashed;
        }
    }
}
