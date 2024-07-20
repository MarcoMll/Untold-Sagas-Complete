using System;
using System.Collections.Generic;

namespace CustomUtilities
{
    public static class RandomUtility
    {
        private static readonly Random Random = new Random();

        public static T GetRandomElement<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentException("The list is null or empty.");
            }

            int index = Random.Next(list.Count);
            return list[index];
        }
        
        public static T GetRandomElement<T>(T[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("The array is null or empty.");
            }

            int index = Random.Next(array.Length);
            return array[index];
        }
    }
}