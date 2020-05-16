using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LR.Tools
{
    public static class NumberExtends
    {
        public static decimal Places(this decimal num, int place = 2)
        {
            return Math.Round(num * 1.0000m, place);
        }
    }

    public static class Encryption
    {
        public static String ToMD5(this string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        static byte[] rgbKey = Encoding.UTF8.GetBytes("c#%^&*(@"), rgbIV = Encoding.UTF8.GetBytes("RGHYU&)K");
        public static string Encrypt(this string str)
        {
            byte[] data = Encoding.Unicode.GetBytes(str);
            using (var descsp = new DESCryptoServiceProvider())
            using (var mStream = new MemoryStream())
            using (var cryptoTransform = descsp.CreateEncryptor(rgbKey, rgbIV))
            using (var cStream = new CryptoStream(mStream, cryptoTransform, CryptoStreamMode.Write))
            {
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                byte[] temp = mStream.ToArray();
                return Convert.ToBase64String(temp);
            }
        }

        public static string Decrypt(this string str)
        {
            byte[] data = Convert.FromBase64String(str);
            using (var descsp = new DESCryptoServiceProvider())
            using (var mStream = new MemoryStream())
            using (var cryptoTransform = descsp.CreateDecryptor(rgbKey, rgbIV))
            using (var cStream = new CryptoStream(mStream, cryptoTransform, CryptoStreamMode.Write))
            {
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                byte[] temp = mStream.ToArray();
                return Encoding.Unicode.GetString(temp);
            }
        }
    }
}
