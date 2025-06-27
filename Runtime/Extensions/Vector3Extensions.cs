using UnityEngine;

namespace VAT.Shared.Extensions
{
    public static class Vector3Extensions
    {
        public static float Max(this Vector3 vector) => Mathf.Max(vector.x, vector.y, vector.z);

        public static Vector3 FlattenNeck(this Vector3 forward, Vector3 up, Vector3? root = null)
        {
            if (!root.HasValue)
            {
                root = Vector3.up;
            }

            return Quaternion.AngleAxis(-90f, root.Value) * Vector3.Cross(root.Value, Quaternion.FromToRotation(up, root.Value) * forward).normalized;
        }

        public static bool Approximately(this Vector3 a, Vector3 b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
        }
    }
}
