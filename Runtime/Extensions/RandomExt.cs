using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace VAT.Shared.Extensions {
    /// <summary>
    /// Extension methods for getting random information.
    /// </summary>
    public static class RandomExtensions {
        /// <summary>
        /// Returns a random value in this list.
        /// </summary>
        /// <typeparam name="T">The list type.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>The random valeu.</returns>
        public static T GetRandom<T>(this IList<T> list) {
            if (list.Count <= 0)
                return default;
            return list[Random.Range(0, list.Count)];
        }
    }
}