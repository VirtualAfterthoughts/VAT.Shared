using UnityEngine;

namespace VAT.Shared.Data
{
    public readonly struct Line
    {
        public Vector3 Start { get; }

        public Vector3 End { get; }

        public Vector3 Center { get; }

        public Line(Vector3 start, Vector3 end)
        {
            Start = start;
            End = end;
            Center = Start - ((Start - end) * 0.5f);
        }

        public readonly Vector3 ClosestPointOnLine(Vector3 point)
        {
            var direction = End - Start;
            float length = direction.magnitude;
            direction.Normalize();

            float projectedLength = Mathf.Clamp(Vector3.Dot(point - Start, direction), 0f, length);

            return Start + direction * projectedLength;
        }
    }
}
