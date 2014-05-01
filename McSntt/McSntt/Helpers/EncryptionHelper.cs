using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace McSntt.Helpers
{
    public class EncryptionHelper
    {
        // This method will return a Sha256 hash as a string given a string as input. 
        // http://stackoverflow.com/questions/12416249/hashing-a-string-with-sha256
        public static string Sha256(string password)
        {
            var crypt = new SHA256Managed();
            var hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            return crypto.Aggregate(hash, (current, bit) => current + bit.ToString("x2"));
        }
 
    }
}