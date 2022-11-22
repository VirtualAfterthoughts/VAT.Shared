using UnityEngine;

namespace VAT.Shared.Extensions
{
    /// <summary>
    /// Extension methods for Vector2s.
    /// </summary>
    public static partial class Vector2Extensions
    {
        /// <summary>
        /// Returns the cross product of a and b.
        /// </summary>
        /// <param name="lhs">The left vector.</param>
        /// <param name="rhs">The right vector.</param>
        /// <returns></returns>
        public static float Cross(this Vector2 lhs, Vector2 rhs) {
            return lhs.x * rhs.y - lhs.y * rhs.x;
        }
    }
}