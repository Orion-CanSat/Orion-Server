using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionServer.Utilities.Crypto
{
    public class MD5
    {
        private static System.Security.Cryptography.MD5 _md5 = System.Security.Cryptography.MD5.Create();

        public static byte[] CreateMD5(byte[] input)
        {
            return _md5.ComputeHash(input);
        }

        public static string CreateMD5(string input)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);

            byte[] hashedBytes = CreateMD5(inputBytes);

            StringBuilder sb = new();
            foreach (byte b in hashedBytes)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }
    }
}
