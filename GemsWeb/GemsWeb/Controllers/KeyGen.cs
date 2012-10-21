using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GemsWeb.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Security.Cryptography;
    using System.Text;
    using System.IO;

    public class KeyGen
    {
        private KeyGen()
        {
        }

        #region "Private variables and Constants"

        private const string seedToUse = "T1GNacCS3213_t3@mO1_GEMS_@SkY1904@";

        /// The salt value used to strengthen the encryption.
        private static readonly byte[] SALT = Encoding.ASCII.GetBytes(seedToUse);
        private static readonly byte[] key;
        private static readonly byte[] initVector;

        private static readonly Rfc2898DeriveBytes rfcKeyGen;
        #endregion

        #region "Constructor"

        static KeyGen()
        {
            //We create the Rfc2898DerveBytes once as Rfc2898DeriveBytes uses a pseudo-random number generator based on HMACSHA1.

            rfcKeyGen = new Rfc2898DeriveBytes(seedToUse, SALT);
            //When calling GetBytes it initializes a new instance of HMAC, Later calls to GetBytes does not need to do this.
            key = rfcKeyGen.GetBytes(32);
            //Generate Initialization Vector - This will be less expensive as we have already intialized Rfc2898DeriveBytes
            initVector = rfcKeyGen.GetBytes(16);
        }

        #endregion


        public static string GeneratePwd(string email)
        {
            //I admit I was bored when I coded this f(x) - Allan
            string userID = email.Split('@')[0];
            string domain = email.Split('@')[1];
            char[] map = new char[] { '@', '|', '^', '#', '%', '$', '=', '*', '+', '&', '~' };

            string pwd = email.Length + "|" + userID.ToString() + "&" + domain;
            string res = Encrypt(pwd);
            int idx = ((2759 + 1904) * (259 + 2012) - (3449 + 1004)) % domain.Length;

            string result =  map[userID.Length % map.Length] + 
                res.Substring(domain.Length % 9, (res.Length * 19 * 4) % 7) + 
                map[idx % map.Length] +
                res.Substring(res.Length - (userID.Length % 9) - 4, userID.Length % 3 + 1) + 
                map[(res.Length * 16 * 11) % map.Length];

            if (result.Length > 12)
                return result.Substring(0, 12);
            else if (result.Length < 8)
                return result + res.Substring(8, 12 - result.Length);
            else
                return result;

            
        }

        public static bool Authenticate(string email, string pwd)
        {
            string ans = GeneratePwd(email);

            return (pwd.CompareTo(ans) == 0);
        }

        #region "Encrypt/Decrypt Methods"

        //Encrypts any string using the Rijndael algorithm.
        //input text: The string to encrypt.
        // returns A Base64 encrypted string.
        public static string Encrypt(string inputText)
        {

            //Create RijndaelManaged cipher for the symmetric algorithm from the key and initVector 
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Key = key;
            rijndaelCipher.IV = initVector;


            byte[] plainText = Encoding.Unicode.GetBytes(inputText);

            using (ICryptoTransform enCrypter = rijndaelCipher.CreateEncryptor())
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, enCrypter, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainText, 0, plainText.Length);
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(memStream.ToArray());
                    }
                }
            }
        }
        #endregion

    }

}