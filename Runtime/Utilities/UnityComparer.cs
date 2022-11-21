using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared.Utilities {
    /// <summary>
    /// Equality comparer for Unity objects.
    /// By default, Dictionaries look up objects by reference and not by instance. 
    /// This means "ContainsKey" may not always return true even when it should.
    /// Setting this as the equality comparer will allow Dictionaries to properly find Unity objects.
    /// </summary>
    public class UnityComparer : IEqualityComparer<Object> {
        public bool Equals(Object x, Object y) => x == y;

        public int GetHashCode(Object x) => x.GetHashCode();
    }
}
