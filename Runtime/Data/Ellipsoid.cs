using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

using static Unity.Mathematics.math;

namespace VAT.Shared.Data
{
    using Unity.Mathematics;

    [Serializable]
    public struct Ellipsoid : IEllipse
    {
        public const int DefaultSegments = 32;

        public float2 radius;
        public float height;

        public IEllipse AsInterface() => this;

        public static Ellipsoid Lerp(Ellipsoid a, Ellipsoid b, float t)
        {
            return new Ellipsoid()
            {
                radius = lerp(a.radius, b.radius, t),
                height = lerp(a.height, b.height, t)
            };
        }

        public void SetRadius(float2 radius)
        {
            this.radius = radius;
        }

        public float2 GetRadius()
        {
            return radius;
        }

        public float GetVolume()
        {
            return AsInterface().GetArea() * height;
        }

        public bool IsInside(SimpleTransform transform, float3 point)
        {
            var local = abs(transform.InverseTransformPoint(point));
            var plane = local.xz < radius;
            return plane.x && plane.y && local.y < height * 0.5f;
        }

        public float3 GetDepenetration(SimpleTransform transform, float3 point)
        {
            if (IsInside(transform, point))
            {
                var local = transform.InverseTransformPoint(point);
                var final = normalize(local) * new float3(radius.x, height * 0.5f, radius.y);
                return transform.TransformDirection(final - local);
            }
            else
                return float3.zero;
        }

#if UNITY_EDITOR
        public void Draw(float3 position, quaternion rotation)
        {
            DrawEllipsoid(radius, height, position, rotation);
        }

        public static void DrawEllipsoid(float2 radius, float height, float3 position, quaternion rotation)
        {
            float angle = 0f;
            float3 lastPoint = float3.zero;
            float3 thisPoint = float3.zero;

            float3 lineUp = new(0.01f, height, 0.01f);
            lineUp = mul(rotation, lineUp);

            float3 fwd = mul(rotation, new float3(0f, 0f, 1f));
            float3 up = mul(rotation, new float3(0f, 1f, 0f));

            rotation = quaternion.LookRotation(up, fwd);

            // Draw the 2D ellipse
            for (int i = 0; i < DefaultSegments + 1; i++)
            {
                thisPoint.x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius.x;
                thisPoint.y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius.y;

                if (i > 0)
                {
                    Gizmos.DrawLine(mul(rotation, lastPoint) + position, mul(rotation, thisPoint) + position);
                }

                lastPoint = thisPoint;
                angle += 360f / DefaultSegments;
            }

            // Draw the height
            float3 midpoint = position;
            midpoint += up * height * 0.5f;

            Gizmos.DrawCube(midpoint, lineUp);
        }

        public bool DrawHandles(float3 position, Quaternion rotation, float2 handleDirections, out float2 radius, out float height, float offset = 0f)
        {
            bool modified = false;
            float offsetSign = offset == 0f ? 1f : Mathf.Sign(offset);

            var color = Handles.color;
            Handles.color = Color.cyan;

            radius = this.radius;
            height = this.height;

            quaternion worldToLocal = inverse(rotation);
            float3 localPosition = mul(worldToLocal, position);

            float3 fwd = mul(rotation, new float3(0f, 0f, 1f));
            float3 up = mul(rotation, new float3(0f, 1f, 0f));
            float3 right = mul(rotation, new float3(1f, 0f, 0f));

            float3 edgeX = position + Mathf.Sign(handleDirections.x) * right * radius.x;
            float3 initialEdgeX = mul(worldToLocal, edgeX);

            EditorGUI.BeginChangeCheck();
            edgeX = Handles.FreeMoveHandle(edgeX, 0.01f, float3.zero, Handles.SphereHandleCap);

            if (EditorGUI.EndChangeCheck())
            {
                float3 localEdgeX = mul(worldToLocal, edgeX);

                radius.x += (localEdgeX.x - initialEdgeX.x) * Mathf.Sign(handleDirections.x);
                radius.x = max(0f, radius.x);

                modified = true;
            }

            float3 edgeY = position + Mathf.Sign(handleDirections.y) * fwd * radius.y;
            float3 initialEdgeY = mul(worldToLocal, edgeY);

            EditorGUI.BeginChangeCheck();
            edgeY = Handles.FreeMoveHandle(edgeY, 0.01f, float3.zero, Handles.SphereHandleCap);

            if (EditorGUI.EndChangeCheck())
            {
                float3 localEdgeY = mul(worldToLocal, edgeY);

                radius.y += (localEdgeY.z - initialEdgeY.z) * Mathf.Sign(handleDirections.y);
                radius.y = max(0f, radius.y);

                modified = true;
            }

            float3 tip = position;
            tip += up * offset * height * 0.5f;
            tip += up * height * 0.5f * offsetSign;

            float3 initialTip = mul(worldToLocal, tip);

            EditorGUI.BeginChangeCheck();

            tip = Handles.FreeMoveHandle(tip, 0.01f, float3.zero, Handles.SphereHandleCap);

            Handles.ArrowHandleCap(0, tip, mul(rotation, Quaternion.AngleAxis(-90f * offsetSign, right)), 0.05f, EventType.Repaint);

            if (EditorGUI.EndChangeCheck())
            {
                float3 localTip = mul(worldToLocal, tip);

                height += (localTip.y - initialTip.y) * offsetSign;
                height = max(0f, height);

                modified = true;
            }

            Handles.color = color;

            return modified;
        }
#endif
    }
}
