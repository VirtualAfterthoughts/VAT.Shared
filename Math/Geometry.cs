using System;

using UnityEngine;

using static Unity.Mathematics.math;

using VAT.Shared.Extensions;

namespace VAT.Shared.Math
{
    using Unity.Mathematics;

    public static partial class Geometry
    {
        /// <summary>
        /// Calculates the area of a triangle using sides A, B, and C
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float TriangleArea(Vector2 a, Vector2 b, Vector2 c) {
            return Mathf.Abs((b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y)) * .5f;
        }
    }
}