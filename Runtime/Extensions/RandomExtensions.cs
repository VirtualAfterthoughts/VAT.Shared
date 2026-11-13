using System;
using System.Collections.Generic;

using Random = UnityEngine.Random;

namespace VAT.Shared.Extensions
{
    /// <summary>
    /// Extension functions for randomization.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Gets a random element from a span.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="span"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this ReadOnlySpan<T> span)
        {
            if (span.Length <= 0)
            {
                return default;
            }

            return span[Random.Range(0, span.Length)];
        }

        /// <summary>
        /// Gets a random element from an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this T[] array) => new ReadOnlySpan<T>(array).GetRandom();

        /// <summary>
        /// Gets a random element from a list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this List<T> list)
        {
            if (list.Count <= 0)
            {
                return default;
            }

            return list[Random.Range(0, list.Count)];
        }
    }
}