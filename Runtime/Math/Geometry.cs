using UnityEngine;

namespace VAT.Shared.Math
{
    /// <summary>
    /// Helper class for solving different geometric problems.
    /// </summary>
    public static partial class Geometry
    {
        /// <summary>
        /// Calculates the area of a triangle.
        /// </summary>
        /// <param name="a">Side a.</param>
        /// <param name="b">Side b.</param>
        /// <param name="c">Side c.</param>
        /// <returns>The area of the triangle.</returns>
        public static float TriangleArea(Vector2 a, Vector2 b, Vector2 c) {
            return Mathf.Abs((b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y)) * .5f;
        }
    }
}