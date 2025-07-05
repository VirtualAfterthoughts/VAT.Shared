using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace VAT.Shared.Data
{
    [Serializable]
    public struct Ellipse
    {
        public Vector2 Radius;

        public Ellipse(Vector2 radius)
        {
            Radius = radius;
        }

        public Ellipse(float a, float b)
        {
            Radius = new Vector2(a, b);
        }

        public static Ellipse operator *(Ellipse ellipse, float scale)
        {
            return new Ellipse()
            {
                Radius = ellipse.Radius * scale,
            };
        }

        public static Ellipse operator *(Ellipse ellipse, Vector2 scale)
        {
            return new Ellipse()
            {
                Radius = ellipse.Radius * scale,
            };
        }

        public static Ellipse operator *(Ellipse a, Ellipse b)
        {
            return new Ellipse()
            {
                Radius = a.Radius * b.Radius,
            };
        }

        public readonly bool IsInside(SimpleTransform transform, Vector3 point)
        {
            var local = transform.InverseTransformPoint(point);
            
            return Mathf.Abs(local.x) < Radius.x && Mathf.Abs(local.z) < Radius.y;
        }

        public readonly Vector3 GetDepenetration(SimpleTransform transform, Vector3 point)
        {
            if (IsInside(transform, point))
            {
                var local = transform.InverseTransformPoint(point);
                var scaled = Vector2.Scale(new Vector2(local.x, local.z), Radius);
                var final = new Vector3(scaled.x, local.y, scaled.y);

                return transform.TransformDirection(final - local);
            }

            return Vector3.zero;
        }

        public readonly Vector3[] GetLocalPoints(float angle = 360f, int segments = 16)
        {
            Vector3[] points = new Vector3[segments + 1];

            float totalAngle = 0f;

            for (int i = 0; i < segments + 1; i++)
            {
                points[i] = new Vector3(Mathf.Sin(Mathf.Deg2Rad * totalAngle) * Radius.x, 0f, Mathf.Cos(Mathf.Deg2Rad * totalAngle) * Radius.y);
                totalAngle += angle / segments;
            }

            return points;
        }
    }
}
