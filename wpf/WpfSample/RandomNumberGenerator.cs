using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSample
{
    /// <summary>
    /// Helper class to generate random numbers
    /// </summary>
    static class RandomNumberGenerator
    {
        static readonly Random _random = new Random();

        /// <summary>
        /// Generates an array of unique, randomly ordered numbers
        /// </summary>
        /// <param name="start">The inclusive first number to return in the array</param>
        /// <param name="count">Count of numbers to generate</param>
        public static int[] Generate(int start, int count)
        {
            return Enumerable.Range(start, count)
                .OrderBy(i => Random(start, start + count))
                .ToArray();
        }

        /// <summary>
        /// Returns a random number from the specified range
        /// </summary>
        /// <param name="lower">Inclusive lower bound</param>
        /// <param name="upper">Inclusive upper bound</param>
        static int Random(int lower, int upper) => _random.Next(lower, upper + 1);

        /// <summary>
        /// Alternative random number generation implementation that generates an array of unique, randomly ordered numbers
        /// (first version seemed too easy!)
        /// </summary>
        /// <param name="start">The inclusive first number to return in the array</param>
        /// <param name="count">Count of numbers to generate</param>
        public static int[] Generate2(int start, int count)
        {
            var set = Enumerable.Range(start, count).ToList();
            var ret = new int[count];

            for (var i = 0; i < count; i++) {
                var index = Random(0, set.Count-1);
                ret[i] = set[index];
                set.RemoveAt(index);
            }
            return ret;
        }
    }
}
