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

#if UNITY_EDITOR
        public void Draw(Vector3 position, Quaternion rotation)
        {
            DrawEllipsoid(Ellipse.Radius, Height, position, rotation);
        }

        public static void DrawEllipsoid(Vector2 radius, float height, Vector3 position, Quaternion rotation)
        {
            float angle = 0f;
            Vector3 lastPoint = Vector3.zero;
            Vector3 thisPoint = Vector3.zero;

            Vector3 lineUp = new(0.01f, height, 0.01f);
            lineUp = rotation * lineUp;

            Vector3 fwd = rotation * Vector3.forward;
            Vector3 up = rotation * Vector3.up;

            rotation = Quaternion.LookRotation(up, fwd);

            // Draw the 2D ellipse
            for (int i = 0; i < DefaultSegments + 1; i++)
            {
                thisPoint.x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius.x;
                thisPoint.y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius.y;

                if (i > 0)
                {
                    Gizmos.DrawLine((rotation * lastPoint) + position, (rotation * thisPoint) + position);
                }

                lastPoint = thisPoint;
                angle += 360f / DefaultSegments;
            }

            // Draw the height
            Vector3 midpoint = position;
            midpoint += up * height * 0.5f;

            Gizmos.DrawCube(midpoint, lineUp);
        }

        public bool DrawHandles(Vector3 position, Quaternion rotation, Vector2 handleDirections, out Vector2 radius, out float height, float offset = 0f)
        {
            bool modified = false;
            float offsetSign = offset == 0f ? 1f : Mathf.Sign(offset);

            var color = Handles.color;
            Handles.color = Color.cyan;

            radius = this.Ellipse.Radius;
            height = this.Height;

            Quaternion worldToLocal = Quaternion.Inverse(rotation);
            Vector3 localPosition = worldToLocal * position;

            Vector3 fwd = rotation * Vector3.forward;
            Vector3 up = rotation * Vector3.up;
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

            Vector3 tip = position;
            tip += up * offset * height * 0.5f;
            tip += up * height * 0.5f * offsetSign;

            Vector3 initialTip = worldToLocal * tip;

            EditorGUI.BeginChangeCheck();

            tip = Handles.FreeMoveHandle(tip, 0.01f, Vector3.zero, Handles.SphereHandleCap);

            Handles.ArrowHandleCap(0, tip, rotation * Quaternion.AngleAxis(-90f * offsetSign, right), 0.05f, EventType.Repaint);

            if (EditorGUI.EndChangeCheck())
            {
                Vector3 localTip = worldToLocal * tip;

                height += (localTip.y - initialTip.y) * offsetSign;
                height = Mathf.Max(0f, height);

                modified = true;
            }

            Handles.color = color;

            return modified;
        }
#endif
    }
}
