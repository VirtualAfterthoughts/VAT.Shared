using UnityEngine;

namespace VAT.Shared.Extensions
{
    public static class Vector3Extensions
    {
        /// <summary>
        /// Returns the maximum component of a Vector3.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float Max(this Vector3 vector) => Mathf.Max(vector.x, vector.y, vector.z);

        public static Vector3 FlattenNeck(this Vector3 forward, Vector3 up, Vector3? root = null)
        {
            if (!root.HasValue)
            {
                root = Vector3.up;
            }

            return Quaternion.AngleAxis(-90f, root.Value) * Vector3.Cross(root.Value, Quaternion.FromToRotation(up, root.Value) * forward).normalized;
        }

        /// <summary>
        /// Compares the components of two vectors using <see cref="Mathf.Approximately(float, float)"/> to account for floating point imprecision.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Approximately(this Vector3 a, Vector3 b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
        }

        /// <summary>
        /// Returns the input vector with the X axis set to the input value.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value"></param>
        /// <returns>(<paramref name="value"/>, <paramref name="vector"/>.y, <paramref name="vector"/>.z)</returns>
        public static Vector3 WithX(this Vector3 vector, float value) => new(value, vector.y, vector.z);

        /// <summary>
        /// Returns the input vector with the Y axis set to the input value.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value"></param>
        /// <returns>(<paramref name="vector"/>.x, <paramref name="value"/>, <paramref name="vector"/>.z)</returns>
        public static Vector3 WithY(this Vector3 vector, float value) => new(vector.x, value, vector.z);

        /// <summary>
        /// Returns the input vector with the Z axis set to the input value.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value"></param>
        /// <returns>(<paramref name="vector"/>.x, <paramref name="vector"/>.y, <paramref name="value"/>)</returns>
        public static Vector3 WithZ(this Vector3 vector, float value) => new(vector.x, vector.y, value);
    }
}
