using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication
{
    class Class1
    {
        public static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            Array.Copy(new byte[32], new byte[8], 8);
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

    }
}
