using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;

namespace evmsService.Controllers
{
    public class OTPGenerator
    {
        // Define default min and max password lengths.
        private static int MAXLENGTH = 12;
        private static int MINLENGTH = 8;

        // Define supported password characters divided into groups.
        private static string LOWERCASE_CHARS = "abcdefghijkmnopqrstwxyz";
        private static string UPPERCASE_CHARS = "ABCDEFGHJKLMNPQRSTWXYZ";
        private static string NUMERIC_CHARS = "1234567890";
        private static string SPECIAL_CHARS = "$@|%";
  
        public static string Generate()
        {
            return Generate(MINLENGTH, MAXLENGTH);
        }

        public static string Generate(int minLength, int maxLength)
        {
            string functionReturnValue = null;
            if ((minLength <= 0 | maxLength <= 0 | minLength > maxLength))
            {
                functionReturnValue = null;
            }

            char[][] charGroups = new char[][] {
			LOWERCASE_CHARS.ToCharArray(),
			UPPERCASE_CHARS.ToCharArray(),
			NUMERIC_CHARS.ToCharArray(),
			SPECIAL_CHARS.ToCharArray()
		};

           
            int[] charsLeftInGroup = new int[charGroups.Length];

           
            int i = 0;
            for (i = 0; i <= charsLeftInGroup.Length - 1; i++)
            {
                charsLeftInGroup[i] = charGroups[i].Length;
            }

            
            int[] leftGroupsOrder = new int[charGroups.Length];

      
            for (i = 0; i <= leftGroupsOrder.Length - 1; i++)
            {
                leftGroupsOrder[i] = i;
            }

            
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = ((randomBytes[0] & 0x7f) << 24 | randomBytes[1] << 16 | randomBytes[2] << 8 | randomBytes[3]);

            // Now, this is real randomization.
            Random random = new Random(seed);

            // This array will hold password characters.
            char[] password = null;

            // Allocate appropriate memory for the password.
            if ((minLength < maxLength))
            {
                password = new char[random.Next(minLength - 1, maxLength) + 1];
            }
            else
            {
                password = new char[minLength];
            }

            // Index of the next character to be added to password.
            int nextCharIdx = 0;

            // Index of the next character group to be processed.
            int nextGroupIdx = 0;

            // Index which will be used to track not processed character groups.
            int nextLeftGroupsOrderIdx = 0;

            // Index of the last non-processed character in a group.
            int lastCharIdx = 0;

            // Index of the last non-processed group.
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

            // Generate password characters one at a time.

            for (i = 0; i <= password.Length - 1; i++)
            {
                if ((lastLeftGroupsOrderIdx == 0))
                {
                    nextLeftGroupsOrderIdx = 0;
                }
                else
                {
                    nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx);
                }

                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                if ((lastCharIdx == 0))
                {
                    nextCharIdx = 0;
                }
                else
                {
                    nextCharIdx = random.Next(0, lastCharIdx + 1);
                }
                password[i] = charGroups[nextGroupIdx][nextCharIdx];
                if ((lastCharIdx == 0))
                {
                    charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
                }
                else
                {
                    if ((lastCharIdx != nextCharIdx))
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] = charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    charsLeftInGroup[nextGroupIdx] = charsLeftInGroup[nextGroupIdx] - 1;
                }
                if ((lastLeftGroupsOrderIdx == 0))
                {
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                }
                else
                {
                   
                    if ((lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx))
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] = leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                                       
                    lastLeftGroupsOrderIdx = lastLeftGroupsOrderIdx - 1;
                }
            }

            functionReturnValue = new string(password);
            return functionReturnValue;
        }
    }
}


