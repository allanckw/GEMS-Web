using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using evmsService.DataAccess;

namespace evmsService.Controllers
{
    public class MobileEncryptor
    {
        const string passPhrase = "2759190468791611";

        public static string encrypt(string toEncrypt)
        {
            using (AesCryptoServiceProvider cryptoSP = getProvider(Encoding.Default.GetBytes(passPhrase)))
            {
                byte[] sourceBytes = Encoding.ASCII.GetBytes(toEncrypt);
                ICryptoTransform transformer = cryptoSP.CreateEncryptor();

                MemoryStream memStream = new MemoryStream();

                CryptoStream crytoStream = new CryptoStream(memStream, transformer, CryptoStreamMode.Write);
                crytoStream.Write(sourceBytes, 0, sourceBytes.Length);
                crytoStream.FlushFinalBlock();

                byte[] encryptedBytes = memStream.ToArray(); 

                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public static string decrypt(string toDecrypt)
        {
            using (AesCryptoServiceProvider acsp = getProvider(Encoding.Default.GetBytes(passPhrase)))
            {
                byte[] RawBytes = Convert.FromBase64String(toDecrypt);
                ICryptoTransform transformer = acsp.CreateDecryptor();

                MemoryStream memStream = new MemoryStream(RawBytes, 0, RawBytes.Length);
                CryptoStream cryptoSteam = new CryptoStream(memStream, transformer, CryptoStreamMode.Read);
                return (new StreamReader(cryptoSteam)).ReadToEnd();
            }
        }

        private static AesCryptoServiceProvider getProvider(byte[] keyPhrase)
        {
            AesCryptoServiceProvider result = new AesCryptoServiceProvider();
            result.BlockSize = 128;
            result.KeySize = 128;
            result.Mode = CipherMode.CBC;
            result.Padding = PaddingMode.PKCS7;

            result.GenerateIV();
            result.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            byte[] key = getKey(keyPhrase, result);
            result.Key = key;
            // result.IV = RealKey;
            return result;
        }

        private static byte[] getKey(byte[] suggestedKey, SymmetricAlgorithm p)
        {
            byte[] rawKey = suggestedKey;
            List<byte> kList = new List<byte>();

            for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
            {
                kList.Add(rawKey[(i / 8) % rawKey.Length]);
            }
            byte[] k = kList.ToArray();
            return k;
        }


    }
}
