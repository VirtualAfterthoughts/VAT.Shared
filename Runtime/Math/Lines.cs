using UnityEngine;

namespace VAT.Shared.Math
{
    public struct LineData
    {
        public Vector3 Start { get; }

        public Vector3 End { get; }

        public Vector3 Center { get; }

        public LineData(Vector3 start, Vector3 end)
        {
            Start = start;
            End = end;
            Center = Start - ((Start - end) * 0.5f);
        }

        public Vector3 ClosestPointOnLine(Vector3 point)
        {
            return point.ClosestPointOnLine(Start, End);
        }
    }

    public static class Lines
    {
        public static Vector3 ClosestPointOnLine(this Vector3 point, Vector3 start, Vector3 end)
        {
            var direction = end - start;
            float length = direction.magnitude;
            direction.Normalize();
            float project_length = Mathf.Clamp(Vector3.Dot(point - start, direction), 0f, length);
            return start + direction * project_length;
        }
    }
}