using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace VAT.Shared.Data
{
    [Serializable]
    public struct Ellipsoid
    {
        public const int DefaultSegments = 32;

        public Ellipse Ellipse;

        public float Height;

        public Ellipsoid(Ellipse ellipse, float height)
        {
            Ellipse = ellipse;
            Height = height;
        }

        public Ellipsoid(Vector2 radius, float height)
        {
            Ellipse = new Ellipse(radius);
            Height = height;
        }

        public Ellipsoid(float a, float b, float height)
        {
            Ellipse = new Ellipse(a, b);
            Height = height;
        }

        public static Ellipsoid Lerp(Ellipsoid a, Ellipsoid b, float t)
        {
            return new Ellipsoid()
            {
                Ellipse = new(Vector2.Lerp(a.Ellipse.Radius, b.Ellipse.Radius, t)),
                Height = Mathf.Lerp(a.Height, b.Height, t)
            };
        }

        public readonly bool IsInside(SimpleTransform transform, Vector3 point)
        {
            var local = transform.InverseTransformPoint(point);

            return Mathf.Abs(local.x) < Ellipse.Radius.x && Mathf.Abs(local.z) < Ellipse.Radius.y && Mathf.Abs(local.y) < Height * 0.5f;
        }

        public readonly Vector3 GetDepenetration(SimpleTransform transform, Vector3 point)
        {
            if (IsInside(transform, point))
            {
                var local = transform.InverseTransformPoint(point);
                var final = Vector3.Scale(local.normalized, new Vector3(Ellipse.Radius.x, Height * 0.5f, Ellipse.Radius.y));
                return transform.TransformDirection(final - local);
            }

            return Vector3.zero;
        }
    }
}
