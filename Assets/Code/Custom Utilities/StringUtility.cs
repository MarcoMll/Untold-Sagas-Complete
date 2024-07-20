using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CustomUtilities
{
    public static class StringUtility
    {
        public static string ReplaceSpaces(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            StringBuilder output = new StringBuilder(input.Length);
            foreach (char character in input)
            {
                if (character == ' ')
                {
                    output.Append('_');
                }
                else
                {
                    output.Append(character);
                }
            }

            return output.ToString();
        }
    }
}