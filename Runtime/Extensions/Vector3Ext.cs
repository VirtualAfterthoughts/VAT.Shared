using UnityEngine;

using static Unity.Mathematics.math;

namespace VAT.Shared.Extensions {
    using VAT.Shared.Data;

    /// <summary>
    /// Extension methods for Vector3s.
    /// </summary>
    public static partial class Vector3Extensions {
        /// <summary>
        /// Returns a vector perpendicular to this vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The perpendicular vector.</returns>
        public static Vector3 Perp(this Vector3 vector) {
            return vector.z < vector.x ? new Vector3(vector.y, -vector.x, 0) : new Vector3(0, -vector.z, vector.y);
        }

        /// <summary>
        /// Gets the angle between two rotations along an axis.
        /// </summary>
        /// <param name="axis">The axis to check.</param>
        /// <param name="from">The first rotation.</param>
        /// <param name="to">The second rotation.</param>
        /// <returns>The axis angle.</returns>
        public static float GetAxisRotation(Vector3 axis, Quaternion from, Quaternion to) {
            var fromAxis = from * axis;
            var toAxis = to * axis;
            return Vector3.SignedAngle(fromAxis, toAxis, -Vector3.Cross(fromAxis, toAxis));
        }

        /// <summary>
        /// Returns the average value of this vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The average component.</returns>
        public static float Average(this Vector3 vector) => FloatExtensions.Average(vector.x, vector.y, vector.z);

        /// <summary>
        /// Returns this vector rounded to the nearest int on each value.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The rounded vector.</returns>
        public static Vector3 Rounded(this Vector3 vector) => new(vector.x.Rounded(), vector.y.Rounded(), vector.z.Rounded());

        /// <summary>
        /// Returns this vector rounded to the nearest place value.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="place">The place value (ex. 10 = tenths place).</param>
        /// <returns>The rounded vector.</returns>
        public static Vector3 Rounded(this Vector3 vector, float place) => Rounded(vector * place) / place;

        /// <summary>
        /// Vector3.Distance except slightly faster by immediately subtracting the vectors and comparing the magnitude.
        /// </summary>
        /// <param name="lft">The left vector.</param>
        /// <param name="rht">The right vector.</param>
        /// <returns>The distance.</returns>
        public static float FastDistance(this Vector3 lft, Vector3 rht) => (rht - lft).magnitude;

        /// <summary>
        /// Calculates the square distance between each vector (faster than regular distance).
        /// </summary>
        /// <param name="lft">The left vector.</param>
        /// <param name="rht">The right vector.</param>
        /// <returns>The square distance.</returns>
        public static float SqrDistance(this Vector3 lft, Vector3 rht) => (rht - lft).sqrMagnitude;

        /// <summary>
        /// Attempts to normalize the vector. If it fails (if the vector is zero), it defaults to supplement.
        /// </summary>
        /// <param name="vector">The vector to normalize.</param>
        /// <param name="supplement">The backup vector.</param>
        /// <returns>The normalized vector.</returns>
        public static Vector3 ForceNormalize(this Vector3 vector, Vector3 supplement) => vector == Vector3.zero ? supplement : vector.normalized;

        /// <summary>
        /// Returns the maximum component of this vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The maximum component.</returns>
        public static float Maximum(this Vector3 vector) => cmax(vector);

        /// <summary>
        /// Returns the minimum component of this vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The minimum component.</returns>
        public static float Minimum(this Vector3 vector) => cmin(vector);

        /// <summary>
        /// Clamps the vector between min and max.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="min">The minimum vector.</param>
        /// <param name="max">The maximum vector.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector3 Clamp(this Vector3 vector, Vector3 min, Vector3 max) => clamp(vector, min, max);

        /// <summary>
        /// Clamps the vector between min and max.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector3 Clamp(this Vector3 vector, float min, float max) => clamp(vector, min, max);

        /// <summary>
        /// Returns the center of two points.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        /// <returns>The center.</returns>
        public static Vector3 Center(this Vector3 p1, Vector3 p2) => p1 + (p2 - p1) / 2;

        /// <summary>
        /// Clamps the angle between two vectors.
        /// </summary>
        /// <param name="lft">The left vector.</param>
        /// <param name="rht">The right vector.</param>
        /// <param name="axis">The axis to clamp along.</param>
        /// <param name="angle">The angle to clamp.</param>
        /// <returns>The clamped lft vector.</returns>
        public static Vector3 ClampAngle(this Vector3 lft, Vector3 rht, Vector3 axis, float angle) {
            // Clamp the angle between valid values.
            angle = Mathf.Clamp(angle, 0f, 180f);

            // Check if we are already between this angle and can ignore the operation.
            if (Vector3.Angle(lft, rht) <= angle)
                return lft;

            // Limit the angles
            Vector3 l = rht;
            float sin = Vector3.SignedAngle(rht, lft, axis);
            axis *= sin;
            Quaternion angleAxis = Quaternion.AngleAxis(angle, axis);
            return angleAxis * l;
        }

        /// <summary>
        /// Flattens the vector in world space.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The flattened vector.</returns>
        public static Vector3 Flatten(this Vector3 vector) {
            vector.y = 0f;
            return vector;
        }

        /// <summary>
        /// Flattens the vector in local space.
        /// </summary>
        /// <param name="vector">The world space vector.</param>
        /// <param name="transform">The transform we are relative to.</param>
        /// <returns>The flattened vector.</returns>
        public static Vector3 FlattenLocal(this Vector3 vector, SimpleTransform transform) {
            vector = transform.InverseTransformPoint(vector);
            vector.y = 0f;
            return transform.TransformPoint(vector);
        }

        /// <summary>
        /// Flattens the direction in local space.
        /// </summary>
        /// <param name="direction">The world space direction.</param>
        /// <param name="transform">The transform we are relative to.</param>
        /// <returns>The flattened direction.</returns>
        public static Vector3 FlattenDirection(this Vector3 direction, SimpleTransform transform) {
            direction = transform.InverseTransformDirection(direction);
            direction.y = 0f;
            return transform.TransformDirection(direction);
        }

        /// <summary>
        /// Flattens the forward vector as if its a neck, using an up vector.
        /// </summary>
        /// <param name="forward">The forward vector.</param>
        /// <param name="up">The up vector.</param>
        /// <param name="root">The relative root vector (ex. the character controller's up). Defaults to world space up.</param>
        /// <returns>The flattened vector.</returns>
        public static Vector3 FlattenNeck(this Vector3 forward, Vector3 up, Vector3? root = null) {
            if (!root.HasValue)
                root = Vector3.up;

            return Quaternion.AngleAxis(-90f, root.Value) * Vector3.Cross(root.Value, Quaternion.FromToRotation(up, root.Value) * forward).normalized;
        }

        /// <summary>
        /// Generate a JSON representation of this Vector3.
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static string ToJson(this Vector3 vector) => JsonUtility.ToJson(vector, false);
    }
}
