using System;

using UnityEngine;

using VAT.Shared.Math;

namespace VAT.Shared.Data
{
    /// <summary>
    /// A serializable ellipse with four dimensions that can curve inward.
    /// </summary>
    [Serializable]
    public struct SoftEllipse : ICurve
    {
        [Tooltip("The radius of the ellipse in the +x direction.")]
        public float PositiveX;

        [Tooltip("The radius of the ellipse in the +y direction.")]
        public float PositiveY;

        [Tooltip("The radius of the ellipse in the -x direction.")]
        public float NegativeX;

        [Tooltip("The radius of the ellipse in the -y direction.")]
        public float NegativeY;

        public SoftEllipse(float a, float b) : this(a, b, a, b) { }

        public SoftEllipse(Vector2 radius) : this(radius.x, radius.y, radius.x, radius.y) { }

        public SoftEllipse(float positiveX, float positiveY, float negativeX, float negativeY)
        {
            PositiveX = positiveX;
            PositiveY = positiveY;
            NegativeX = negativeX;
            NegativeY = negativeY;
        }

        public readonly Vector3 GetPointOnCurve(float angle)
        {
            var quadrant = AngleMath.GetQuadrantFromDegrees(angle);

            var (xRadius, yRadius) = AngleMath.GetValuesFromQuadrant(quadrant, PositiveX, PositiveY, NegativeX, NegativeY);

            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * xRadius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * yRadius;

            return new Vector3(x, 0f, y);
        }

        public static SoftEllipse operator *(SoftEllipse ellipse, float scale) => new(ellipse.PositiveX * scale, ellipse.PositiveY * scale, ellipse.NegativeX * scale, ellipse.NegativeY * scale);
        public static SoftEllipse operator *(SoftEllipse a, SoftEllipse b) => new(a.PositiveX * b.PositiveX, a.PositiveY * b.PositiveY, a.NegativeX * b.NegativeX, a.NegativeY * b.NegativeY);
    }
}
