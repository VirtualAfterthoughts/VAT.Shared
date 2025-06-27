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
        public const int DefaultSegments = 32;

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

        public Vector3[] GetLocalPoints(int segments = DefaultSegments)
        {
            Vector3[] points = new Vector3[segments + 1];

            float angle = 0f;

            for (int i = 0; i < segments + 1; i++)
            {
                points[i] = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle) * Radius.x, 0f, Mathf.Cos(Mathf.Deg2Rad * angle) * Radius.y);
                angle += 360f / segments;
            }

            return points;
        }

#if UNITY_EDITOR
        public void Draw(Vector3 position, Quaternion rotation)
        {
            DrawEllipse(Radius, position, rotation);
        }

        public static void DrawEllipse(Vector2 radius, Vector3 position, Quaternion rotation)
        {
            var ellipse = new Ellipse() { Radius = radius };
            var points = ellipse.GetLocalPoints();

            for (var i = 0; i < points.Length; i++)
            {
                if (i > 0)
                {
                    Gizmos.DrawLine((rotation * points[i - 1]) + position, (rotation * points[i]) + position);
                }
            }
        }

        public bool DrawHandles(Vector3 position, Quaternion rotation, Vector2 handleDirections, out Vector2 radius)
        {
            bool modified = false;

            var color = Handles.color;
            Handles.color = Color.cyan;

            radius = this.Radius;

            Quaternion worldToLocal = Quaternion.Inverse(rotation);
            Vector3 localPosition = worldToLocal * position;

            Vector3 fwd = rotation * Vector3.forward;
            Vector3 right = rotation * Vector3.right;

            Vector3 edgeX = position + Mathf.Sign(handleDirections.x) * right * radius.x;
            Vector3 initialEdgeX = worldToLocal * edgeX;

            EditorGUI.BeginChangeCheck();
            edgeX = Handles.FreeMoveHandle(edgeX, 0.01f, Vector3.zero, Handles.SphereHandleCap);

            if (EditorGUI.EndChangeCheck())
            {
                Vector3 localEdgeX = worldToLocal * edgeX;

                radius.x += (localEdgeX.x - initialEdgeX.x) * Mathf.Sign(handleDirections.x);
                radius.x = Mathf.Max(0f, radius.x);

                modified = true;
            }

            Vector3 edgeY = position + Mathf.Sign(handleDirections.y) * fwd * radius.y;
            Vector3 initialEdgeY = worldToLocal * edgeY;

            EditorGUI.BeginChangeCheck();
            edgeY = Handles.FreeMoveHandle(edgeY, 0.01f, Vector3.zero, Handles.SphereHandleCap);

            if (EditorGUI.EndChangeCheck())
            {
                Vector3 localEdgeY = worldToLocal * edgeY;

                radius.y += (localEdgeY.z - initialEdgeY.z) * Mathf.Sign(handleDirections.y);
                radius.y = Mathf.Max(0f, radius.y);

                modified = true;
            }

            Handles.color = color;

            return modified;
        }
#endif
    }
}
