using System;

using UnityEngine;

using VAT.Shared.Data;

namespace VAT.Shared.Extensions
{
    /// <summary>
    /// Extension functions for curves.
    /// </summary>
    public static class CurveExtensions
    {
        /// <summary>
        /// Gets the vertices of the curve in local space and writes them to a span.
        /// </summary>
        /// <param name="points">The span that will be written to. The length is equal to the segment count.</param>
        public static void GetVertices(this ICurve curve, Span<Vector3> points) => curve.GetVertices(points, 360f);
    }
}
