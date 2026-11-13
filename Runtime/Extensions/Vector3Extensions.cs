using UnityEngine;

namespace VAT.Shared.Extensions
{
    /// <summary>
    /// Extension functions for Unity Vector3s.
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary>
        /// Returns the largest component of a Vector3.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float Max(this Vector3 vector) => Mathf.Max(vector.x, vector.y, vector.z);

        /// <summary>
        /// Flattens a forward vector into world up based on its related up vector.
        /// <para>
        /// This can be useful for flattening a transform that can flip its rotation upside down, 
        /// as it'll keep the facing forward direction rather than flipping around like the naive flattening implementation.
        /// </para>
        /// <para>
        /// However, there can still be issues when rolling the rotation that have to be accounted for manually.
        /// </para>
        /// </summary>
        /// <param name="forward">The forward vector that will be flattened.</param>
        /// <param name="up">The up vector that is used to determine the pitch.</param>
        /// <returns></returns>
        public static Vector3 FlattenRotation(this Vector3 forward, Vector3 up) => FlattenRotation(forward, up, Vector3.up);

        /// <summary>
        /// Flattens a forward vector based on its related up vector and the normal of the plane it is flattened into.
        /// <para>
        /// This can be useful for flattening a transform that can flip its rotation upside down, 
        /// as it'll keep the facing forward direction rather than flipping around like the naive flattening implementation.
        /// </para>
        /// <para>
        /// However, there can still be issues when rolling the rotation that have to be accounted for manually.
        /// </para>
        /// </summary>
        /// <param name="forward">The forward vector that will be flattened.</param>
        /// <param name="up">The up vector that is used to determine the pitch.</param>
        /// <param name="normal">The normal vector that acts as the "up" direction the forward vector is flattened into.</param>
        /// <returns></returns>
        public static Vector3 FlattenRotation(this Vector3 forward, Vector3 up, Vector3 normal)
        {
            // Get the current right direction from the forward and up vectors
            var right = Vector3.Cross(up, forward);

            // Get the new forward direction using the right direction and the normal as the up
            var flatForward = Vector3.Cross(right, normal);

            return flatForward;
        }

        /// <summary>
        /// Compares two Vector3 values and returns true if their floating point representations match exactly.
        /// <para>This will have issues with imprecision similar to the default float equality check.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Exactly(this Vector3 a, Vector3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

        /// <summary>
        /// Compares two Vector3 values and returns true if they are similar.
        /// <para>This is the same as the default Vector3 equality operator, just more explicitly an approximation check.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Approximately(this Vector3 a, Vector3 b) => a == b;

        /// <summary>
        /// Returns the input vector with the X axis set to the input value.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value"></param>
        /// <returns>(<paramref name="value"/>, <paramref name="vector"/>.y, <paramref name="vector"/>.z)</returns>
        public static Vector3 WithX(this Vector3 vector, float value)
        {
            vector.x = value;
            return vector;
        }

        /// <summary>
        /// Returns the input vector with the Y axis set to the input value.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value"></param>
        /// <returns>(<paramref name="vector"/>.x, <paramref name="value"/>, <paramref name="vector"/>.z)</returns>
        public static Vector3 WithY(this Vector3 vector, float value)
        {
            vector.y = value;
            return vector;
        }

        /// <summary>
        /// Returns the input vector with the Z axis set to the input value.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value"></param>
        /// <returns>(<paramref name="vector"/>.x, <paramref name="vector"/>.y, <paramref name="value"/>)</returns>
        public static Vector3 WithZ(this Vector3 vector, float value)
        {
            vector.z = value;
            return vector;
        }
    }
}
