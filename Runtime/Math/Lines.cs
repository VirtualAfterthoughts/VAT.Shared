using UnityEngine;

namespace VAT.Shared.Math
{
    /// <summary>
    /// A structure for storing information about a line.
    /// </summary>
    public struct LineData
    {
        /// <summary>
        /// The start of the line.
        /// </summary>
        public Vector3 Start { get; }

        /// <summary>
        /// The end of the line.
        /// </summary>
        public Vector3 End { get; }

        /// <summary>
        /// The center of the line.
        /// </summary>
        public Vector3 Center { get; }

        /// <summary>
        /// Is this line valid?
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Creates a new line.
        /// </summary>
        /// <param name="start">The start of the line.</param>
        /// <param name="end">The end of the line.</param>
        public LineData(Vector3 start, Vector3 end) {
            Start = start;
            End = end;
            Center = Start - ((Start - end) * 0.5f);
            IsValid = true;
        }

        /// <summary>
        /// Calculates the closest point on this line.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The closest point.</returns>
        public Vector3 ClosestPointOnLine(Vector3 point) {
            return point.ClosestPointOnLine(Start, End);
        }
    }

    /// <summary>
    /// Helper class for calculating line information.
    /// </summary>
    public static partial class Lines {
        /// <summary>
        /// Calculates the closest point on a line.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="start">The start of the line.</param>
        /// <param name="end">The end of the line.</param>
        /// <returns>The closest point.</returns>
        public static Vector3 ClosestPointOnLine(this Vector3 point, Vector3 start, Vector3 end) {
            var direction = end - start;
            float length = direction.magnitude;
            direction.Normalize();
            float project_length = Mathf.Clamp(Vector3.Dot(point - start, direction), 0f, length);
            return start + direction * project_length;
        }
    }
}