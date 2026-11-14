using System;

using UnityEngine;

namespace VAT.Shared.Data
{
    /// <summary>
    /// A serializable ellipse.
    /// </summary>
    [Serializable]
    public struct Ellipse : ICurve
    {
        [Tooltip("The radius of the ellipse.")]
        public Vector2 Radius;

        public Ellipse(Vector2 radius)
        {
            Radius = radius;
        }

        public Ellipse(float a, float b) : this(new Vector2(a, b)) { }

        /// <summary>
        /// Returns true if a point is inside the ellipse given the ellipse's transform.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public readonly bool IsInside(SimpleTransform transform, Vector3 point)
        {
            var local = transform.InverseTransformPoint(point);
            
            return Mathf.Abs(local.x) < Radius.x && Mathf.Abs(local.z) < Radius.y;
        }

        /// <summary>
        /// Gets the translation required to push a point out of the ellipse onto its surface, or zero if the point is outside the ellipse.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public readonly Vector3 GetDepenetration(SimpleTransform transform, Vector3 point)
        {
            if (!IsInside(transform, point))
            {
                return Vector3.zero;
            }

            var local = transform.InverseTransformPoint(point);
            var scaled = Vector2.Scale(new Vector2(local.x, local.z), Radius);
            var final = new Vector3(scaled.x, local.y, scaled.y);

            return transform.TransformDirection(final - local);
        }

        public readonly Vector3 GetPointOnCurve(float angle)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * Radius.x;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * Radius.y;

            return new Vector3(x, 0f, y);
        }

        public static Ellipse operator *(Ellipse ellipse, float scale) => new(ellipse.Radius * scale);
        public static Ellipse operator *(Ellipse ellipse, Vector2 scale) => new(ellipse.Radius * scale);
        public static Ellipse operator *(Ellipse a, Ellipse b) => new(a.Radius * b.Radius);
    }
}
