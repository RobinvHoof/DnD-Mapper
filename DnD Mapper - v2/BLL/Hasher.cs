using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BLL
{
    static public class Hasher
    {
        /// <summary>
        /// Creates the Hash for the given string
        /// </summary>
        /// <param name="input">string to Hash</param>
        /// <returns>Hash value</returns>
        public static string CreateHash(string input)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashValue = SHA256.Create().ComputeHash(inputBytes);

            StringBuilder stringBuilder = new StringBuilder();

            foreach(byte b in hashValue) {
                stringBuilder.AppendFormat("{0:X2}", b);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Compares a given Hash to a given string
        /// </summary>
        /// <param name="input">String to be compared</param>
        /// <param name="hash">Hash value to be compared to</param>
        /// <returns>Result of the comparison</returns>
        public static bool CompareStringToHash(string input, string hash)
        {
            return (CreateHash(input) == hash);
        }
    }
}
