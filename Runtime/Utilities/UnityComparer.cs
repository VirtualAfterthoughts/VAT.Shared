using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared.Utilities
{
    /// <summary>
    /// An equality comparer for a collection that compares Unity objects properly.
    /// </summary>
    public class UnityComparer : IEqualityComparer<Object>
    {
        public bool Equals(Object x, Object y)
        {
            return x == y;
        }

        public int GetHashCode(Object x)
        {
            return x.GetHashCode();
        }
    }
}
