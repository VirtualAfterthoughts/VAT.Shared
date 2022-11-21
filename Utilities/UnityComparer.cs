using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared.Utilities {
    /// <summary>
    /// Equality comparer for unity objects.
    /// </summary>
    public class UnityComparer : IEqualityComparer<Object> {
        public bool Equals(Object x, Object y) => x == y;

        public int GetHashCode(Object x) => x.GetHashCode();
    }
}
