using UnityEngine;

using VAT.Shared.Data;

namespace VAT.Shared.Utilities
{
    public static class EllipsoidGizmos
    {
        public static void DrawEllipsoid(Ellipsoid ellipsoid, Vector3 position, Vector3 axis) => DrawEllipsoid(ellipsoid, 360f, position, axis);

        public static void DrawEllipsoid(Ellipsoid ellipsoid, float angle, Vector3 position, Vector3 axis) => DrawEllipsoid(ellipsoid.Ellipse.Radius, ellipsoid.Height, angle, position, axis);

        public static void DrawEllipsoid(Vector2 radius, float height, Vector3 position, Vector3 axis) => DrawEllipsoid(radius, height, 360f, position, axis);

        public static void DrawEllipsoid(Vector2 radius, float height, float angle, Vector3 position, Vector3 axis)
        {
            var rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.up) * Quaternion.LookRotation(axis);

            var forward = rotation * Vector3.forward;
            var right = rotation * Vector3.right;

            var halfHeight = height * 0.5f;

            var top = position + axis * halfHeight;
            var bottom = position - axis * halfHeight;

            EllipseGizmos.DrawEllipse(radius, angle, top, rotation);

            EllipseGizmos.DrawEllipse(radius, angle, bottom, rotation);

            float edgeAngle = angle / 4f;

            Vector3 edgeOne = rotation * new Vector3(Mathf.Sin(0f) * radius.x, 0f, Mathf.Cos(0f) * radius.y);
            Vector3 edgeTwo = rotation * new Vector3(Mathf.Sin(Mathf.Deg2Rad * edgeAngle) * radius.x, 0f, Mathf.Cos(Mathf.Deg2Rad * edgeAngle) * radius.y);
            Vector3 edgeThree = rotation * new Vector3(Mathf.Sin(Mathf.Deg2Rad * edgeAngle * 2f) * radius.x, 0f, Mathf.Cos(Mathf.Deg2Rad * edgeAngle * 2f) * radius.y);
            Vector3 edgeFour = rotation * new Vector3(Mathf.Sin(Mathf.Deg2Rad * edgeAngle * 3f) * radius.x, 0f, Mathf.Cos(Mathf.Deg2Rad * edgeAngle * 3f) * radius.y);
            Vector3 edgeFive = rotation * new Vector3(Mathf.Sin(Mathf.Deg2Rad * edgeAngle * 4f) * radius.x, 0f, Mathf.Cos(Mathf.Deg2Rad * edgeAngle * 4f) * radius.y);

            Gizmos.DrawLine(top + edgeOne, bottom + edgeOne);
            Gizmos.DrawLine(top + edgeTwo, bottom + edgeTwo);
            Gizmos.DrawLine(top + edgeThree, bottom + edgeThree);
            Gizmos.DrawLine(top + edgeFour, bottom + edgeFour);
            Gizmos.DrawLine(top + edgeFive, bottom + edgeFive);
        }
    }
}
