using UnityEngine;

namespace VAT.Shared.Data
{
    /// <summary>
    /// A 3D line segment.
    /// </summary>
    public readonly struct Line
    {
        /// <summary>
        /// The line's start.
        /// </summary>
        public Vector3 Start { get; }

        /// <summary>
        /// The line's end.
        /// </summary>
        public Vector3 End { get; }

        /// <summary>
        /// The line's center.
        /// </summary>
        public Vector3 Center { get; }

        /// <summary>
        /// Creates a line from a start and end point.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Line(Vector3 start, Vector3 end)
        {
            Start = start;
            End = end;
            Center = Start - ((Start - end) * 0.5f);
        }

        /// <summary>
        /// Returns the closest point on the line to a point in space.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public readonly Vector3 ClosestPointOnLine(Vector3 point)
        {
            var direction = End - Start;
            float length = direction.magnitude;
            direction.Normalize();

            float projectedLength = Mathf.Clamp(Vector3.Dot(point - Start, direction), 0f, length);

            return Start + direction * projectedLength;
        }
    }
}
