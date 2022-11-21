using UnityEngine;

namespace VAT.Shared.Extensions
{
    public static partial class Vector2Extensions
    {
        /// <summary>
        /// Returns the cross product of a and b.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static float Cross(this Vector2 lhs, Vector2 rhs)
        {
            return lhs.x * rhs.y - lhs.y * rhs.x;
        }
    }
}