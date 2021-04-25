using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Lucid.Views
{
    class Hash
    {
        public string PassHash(string data)
        {
            SHA1 sha = SHA1.Create();
            byte[] hashData = sha.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder();

            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            return returnValue.ToString();
        }
    }
}
