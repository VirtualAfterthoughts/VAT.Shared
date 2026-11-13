using System;

using UnityEngine;

namespace VAT.Shared.Data
{
    /// <summary>
    /// A serializable elliptic cylinder.
    /// </summary>
    [Serializable]
    public struct EllipticCylinder
    {
        /// <summary>
        /// The base ellipse.
        /// </summary>
        public Ellipse Ellipse;

        /// <summary>
        /// The height of the cylinder.
        /// </summary>
        public float Height;

        public EllipticCylinder(Ellipse ellipse, float height)
        {
            Ellipse = ellipse;
            Height = height;
        }

        public EllipticCylinder(Vector2 radius, float height) : this(new Ellipse(radius), height) { }

        public EllipticCylinder(float a, float b, float height) : this(new Ellipse(a, b), height) { }

        /// <summary>
        /// Linearly interpolates between elliptic cylinders a and b by t.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static EllipticCylinder Lerp(EllipticCylinder a, EllipticCylinder b, float t)
        {
            return new EllipticCylinder()
            {
                Ellipse = new(Vector2.Lerp(a.Ellipse.Radius, b.Ellipse.Radius, t)),
                Height = Mathf.Lerp(a.Height, b.Height, t)
            };
        }

        /// <summary>
        /// Returns true if a point is inside the elliptic cylinder given the cylinder's transform.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public readonly bool IsInside(SimpleTransform transform, Vector3 point)
        {
            var local = transform.InverseTransformPoint(point);

            return Mathf.Abs(local.x) < Ellipse.Radius.x && Mathf.Abs(local.z) < Ellipse.Radius.y && Mathf.Abs(local.y) < Height * 0.5f;
        }

        /// <summary>
        /// Gets the translation required to push a point out of the elliptic cylinder onto its surface, or zero if the point is outside the cylinder.
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
            var final = Vector3.Scale(local.normalized, new Vector3(Ellipse.Radius.x, Height * 0.5f, Ellipse.Radius.y));
            return transform.TransformDirection(final - local);
        }
    }
}
