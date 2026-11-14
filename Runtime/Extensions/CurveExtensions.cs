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
        /// <param name="curve"></param>
        /// <param name="points">The span that will be written to. The length is equal to the segment count.</param>
        public static void GetVertices(this ICurve curve, Span<Vector3> points) => curve.GetVertices(points, 360f);

        /// <summary>
        /// Gets the vertices of the curve in local space and writes them to a span.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="points">The span that will be written to. The length is equal to the segment count.</param>
        /// <param name="angle">The maximum angle of the curve.</param>
        public static void GetVertices(this ICurve curve, Span<Vector3> points, float angle)
        {
            int segmentCount = points.Length;

            float segmentAngle = 0f;

            if (segmentCount > 1)
            {
                segmentAngle = angle / (segmentCount - 1);
            }

            float totalAngle = 0f;

            for (int i = 0; i < segmentCount; i++)
            {
                points[i] = curve.GetPointOnCurve(totalAngle);
                totalAngle += segmentAngle;
            }
        }
    }
}
