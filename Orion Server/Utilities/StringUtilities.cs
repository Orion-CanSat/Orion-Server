#nullable enable

using System;
using System.Linq;

namespace OrionServer.Utilities
{
    public class StringUtilities
    {
        public static readonly string ASCIIChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        
        /// <summary>
        /// Generates random strings.
        /// Solution by dtb on StackOverflow (url: https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings)
        /// </summary>
        /// <param name="stringLength">Number of characters that the random string need to be</param>
        /// <returns>Random string</returns>
        // ToDo: Introduce RNGCryptoServiceProvider for secure random number string generator
        public static string GenerateRandomString(int stringLength)
        {
            Random rnd = new Random(Environment.TickCount & Int32.MaxValue);
            return new string(Enumerable.Repeat(StringUtilities.ASCIIChars, stringLength)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
    }
}