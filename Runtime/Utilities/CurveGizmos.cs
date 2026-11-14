using UnityEngine;

using VAT.Shared.Data;
using VAT.Shared.Extensions;

namespace VAT.Shared.Utilities
{
    /// <summary>
    /// Functions for drawing <see cref="ICurve"/> gizmos.
    /// </summary>
    public static class CurveGizmos
    {
        /// <summary>
        /// The amount of segments that curves are drawn with.
        /// </summary>
        public static readonly int CurveSegments = 16;

        /// <summary>
        /// Draws a curve at a given position and rotation.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public static void DrawCurve(ICurve curve, Vector3 position, Quaternion rotation) => DrawCurve(curve, 360f, position, rotation);

        /// <summary>
        /// Draws a curve with a maximum angle at a given position and rotation.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="angle"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public static void DrawCurve(ICurve curve, float angle, Vector3 position, Quaternion rotation)
        {
            var points = new Vector3[CurveSegments];

            curve.GetVertices(points, angle);

            for (var i = 0; i < points.Length; i++)
            {
                if (i > 0)
                {
                    Gizmos.DrawLine((rotation * points[i - 1]) + position, (rotation * points[i]) + position);
                }
            }
        }

        /// <summary>
        /// Draws two connected curves.
        /// </summary>
        /// <param name="startCurve"></param>
        /// <param name="startTransform"></param>
        /// <param name="endCurve"></param>
        /// <param name="endTransform"></param>
        public static void DrawCurves(ICurve startCurve, SimpleTransform startTransform, ICurve endCurve, SimpleTransform endTransform) => DrawCurves(startCurve, startTransform, endCurve, endTransform, 360f);

        /// <summary>
        /// Draws two connected curves with a maximum angle.
        /// </summary>
        /// <param name="startCurve"></param>
        /// <param name="startTransform"></param>
        /// <param name="endCurve"></param>
        /// <param name="endTransform"></param>
        /// <param name="angle"></param>
        public static void DrawCurves(ICurve startCurve, SimpleTransform startTransform, ICurve endCurve, SimpleTransform endTransform, float angle)
        {
            DrawCurve(startCurve, angle, startTransform.Position, startTransform.Rotation);

            DrawCurve(endCurve, angle, endTransform.Position, endTransform.Rotation);

            var startEdges = GetEdgeVertices(startCurve, angle);

            var endEdges = GetEdgeVertices(endCurve, angle);

            for (var i = 0; i < startEdges.Length; i++)
            {
                var startEdge = startTransform.Position + startTransform.Rotation * startEdges[i];
                var endEdge = endTransform.Position + endTransform.Rotation * endEdges[i];

                Gizmos.DrawLine(startEdge, endEdge);
            }
        }

        private static Vector3[] GetEdgeVertices(ICurve curve, float angle)
        {
            Vector3[] edgeVertices = new Vector3[5];

            float edgeAngle = angle / (edgeVertices.Length - 1);

            for (var i = 0; i < edgeVertices.Length; i++)
            {
                edgeVertices[i] = curve.GetPointOnCurve(edgeAngle * i);
            }

            return edgeVertices;
        }
    }
}
