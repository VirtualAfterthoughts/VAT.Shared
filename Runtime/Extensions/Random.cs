using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace VAT.Shared.Extensions {
    public static class RandomExtensions {
        public static T GetRandom<T>(this IList<T> c) {
            if (c.Count <= 0)
                return default;
            return c[Random.Range(0, c.Count)];
        }
    }
}