using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared.Utilities
{
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
