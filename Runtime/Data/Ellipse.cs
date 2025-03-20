using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

using static Unity.Mathematics.math;

using VAT.Shared.Math;

namespace VAT.Shared.Data
{
    using Unity.Mathematics;

    public struct EllipseCylinderMesh
    {
        public Ellipse bottom;
        public SimpleTransform bottomTransform;
        public bool isBottomFilled;

        public Ellipse top;
        public SimpleTransform topTransform;
        public bool isTopFilled;

        public int segments;

        public MeshDescriptor CreateDescriptor()
        {
            if (segments <= 0)
                segments = 32;

            List<Vector3> verticies = new List<Vector3>();
            List<MeshTriangle> triangles = new List<MeshTriangle>();

            segments = 16;

            foreach (var point in bottom.GetLocalPoints(segments))
            {
                verticies.Add(mul(bottomTransform.Rotation, point) + bottomTransform.Position);
            }

            int offset = verticies.Count;

            foreach (var point in top.GetLocalPoints(segments))
            {
                verticies.Add(mul(topTransform.Rotation, point) + topTransform.Position);
            }

            int triangleIndex = 1;

            for (var i = 0; i < offset; i++)
            {
                if (i > 0)
                {
                    triangles.Add(new MeshTriangle(triangleIndex - 1, triangleIndex, triangleIndex + offset));
                    triangles.Add(new MeshTriangle(triangleIndex - 1 + offset, triangleIndex - 1, triangleIndex + offset));
                    triangleIndex += 1;
                }
            }

            if (isBottomFilled)
            {
                verticies.Add(bottomTransform.Position);

                int holeIndex = 1;

                for (var i = 0; i < offset - 1; i++)
                {
                    triangles.Add(new MeshTriangle(holeIndex - 1, verticies.Count - 1, holeIndex));
                    holeIndex += 1;
                }
            }

            if (isTopFilled)
            {
                verticies.Add(topTransform.Position);

                int holeIndex = 1;

                for (var i = 0; i < offset - 1; i++)
                {
                    triangles.Add(new MeshTriangle(verticies.Count - 1, holeIndex + offset - 1, holeIndex + offset));
                    holeIndex += 1;
                }
            }

            return new MeshDescriptor(verticies, triangles);
        }
    }

    [Serializable]
    public struct Ellipse : IEllipse
    {
        public const int DefaultSegments = 32;

        public float2 radius;

        public IEllipse AsInterface() => this;

        public void SetRadius(float2 radius)
        {
            this.radius = radius;
        }

        public float2 GetRadius()
        {
            return radius;
        }

        public Ellipse Scaled(float scale)
        {
            return new Ellipse()
            {
                radius = radius * scale
            };
        }

        public Ellipse Scaled(float2 scale)
        {
            return new Ellipse()
            {
                radius = radius * scale
            };
        }

        public bool IsInside(SimpleTransform transform, float3 point)
        {
            var local = abs(transform.InverseTransformPoint(point));
            var plane = local.xz < radius;
            return plane.x && plane.y;
        }

        public float3 GetDepenetration(SimpleTransform transform, float3 point)
        {
            if (IsInside(transform, point))
            {
                var local = transform.InverseTransformPoint(point);
                var scaled = normalize(local.xz) * radius;
                var final = new float3(scaled.x, local.y, scaled.y);

                return transform.TransformVector(final - local);
            }
            else
                return float3.zero;
        }

        public float3[] GetLocalPoints(int segments = DefaultSegments)
        {
            float3[] points = new float3[segments + 1];

            float angle = 0f;

            for (int i = 0; i < segments + 1; i++)
            {
                points[i] = new float3(sin(Mathf.Deg2Rad * angle) * radius.x, 0f, cos(Mathf.Deg2Rad * angle) * radius.y);
                angle += 360f / segments;
            }

            return points;
        }

#if UNITY_EDITOR
        public void Draw(float3 position, quaternion rotation)
        {
            DrawEllipse(radius, position, rotation);
        }

        public static void DrawEllipse(float2 radius, float3 position, quaternion rotation)
        {
            var ellipse = new Ellipse() { radius = radius };
            var points = ellipse.GetLocalPoints();

            for (var i = 0; i < points.Length; i++)
            {
                if (i > 0)
                {
                    Gizmos.DrawLine(mul(rotation, points[i - 1]) + position, mul(rotation, points[i]) + position);
                }
            }
        }

        public bool DrawHandles(float3 position, Quaternion rotation, float2 handleDirections, out float2 radius)
        {
            bool modified = false;

            var color = Handles.color;
            Handles.color = Color.cyan;

            radius = this.radius;

            quaternion worldToLocal = inverse(rotation);
            float3 localPosition = mul(worldToLocal, position);

            float3 fwd = mul(rotation, new float3(0f, 0f, 1f));
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

            Handles.color = color;

            return modified;
        }
#endif
    }
}
