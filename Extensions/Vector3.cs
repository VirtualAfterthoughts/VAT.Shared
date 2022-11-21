using UnityEngine;

using static Unity.Mathematics.math;

namespace VAT.Shared.Extensions {
    using Unity.Mathematics;

    public static partial class Vector3Extensions {
        /// <summary>
        /// Returns a vector perpendicular to this vector.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 Perp(this Vector3 vector) {
            return vector.z < vector.x ? new Vector3(vector.y, -vector.x, 0) : new Vector3(0, -vector.z, vector.y);
        }

        /// <summary>
        /// Gets the angle between two rotations along an axis.
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float GetAxisRotation(Vector3 axis, Quaternion from, Quaternion to) {
            var fromAxis = from * axis;
            var toAxis = to * axis;
            return Vector3.SignedAngle(fromAxis, toAxis, -Vector3.Cross(fromAxis, toAxis));
        }

        // Gets the average value of the vector3 (not the length).
        public static float Average(this Vector3 vector) => FloatExtensions.Average(vector.x, vector.y, vector.z);

        // Returns this vector rounded to the nearest int on each axis.
        public static Vector3 Rounded(this Vector3 vector) => new Vector3(vector.x.Rounded(), vector.y.Rounded(), vector.z.Rounded());

        // Returns this vector rounded to the nearest place value (ex. 10ths place = 10)
        public static Vector3 Rounded(this Vector3 vector, float p) => Rounded(vector * p) / p;

        // Apparently Vector3.Distance is slower than just getting the magnitude...
        public static float FastDistance(this Vector3 lft, Vector3 rht) => (rht - lft).magnitude;

        // Gets the distance between two vectors as a sqr (use only for comparison!)
        public static float SqrDistance(this Vector3 lft, Vector3 rht) => (rht - lft).sqrMagnitude;

        // Returns supplement if Vector3 is zero
        public static Vector3 ForceNormalize(this Vector3 vector, Vector3 supplement) => vector == Vector3.zero ? supplement : vector.normalized;

        // Returns the maximum comp of a vector3
        public static float Maximum(this Vector3 vector) => cmax(vector);

        // Returns the smallest comp of a vector3
        public static float Minimum(this Vector3 vector) => Mathf.Min(vector[0], vector[1], vector[2]);

        // Clamps the vector between min and max
        public static Vector3 Clamp(this Vector3 vector, Vector3 min, Vector3 max) => Vector3.Min(Vector3.Max(vector, min), max);

        // Clamps the vector between floats min and max
        public static Vector3 Clamp(this Vector3 vector, float min, float max) => vector.Clamp(Vector3.one * min, Vector3.one * max);

        // Returns the center of two points
        public static Vector3 Center(this Vector3 point1, Vector3 point2) => point1 + (point2 - point1) / 2;

        // Clamps the angle between two vectors
        public static Vector3 ClampAngle(this Vector3 lft, Vector3 rht, Vector3 axis, float angle = 90f) {
            //Clamp Angle Value
            angle = Mathf.Clamp(angle, 0f, 180f);
            //Between?
            if (Vector3.Angle(lft, rht) <= angle)
                return lft;
            //Limit
            Vector3 l = rht;
            float sin = Vector3.SignedAngle(rht, lft, axis);
            axis *= sin;
            Quaternion angleAxis = Quaternion.AngleAxis(angle, axis);
            return angleAxis * l;
        }

        // Flattens a vector
        public static Vector3 Flatten(this Vector3 vector) {
            vector.y = 0f;
            return vector;
        }

        public static Vector3 FlattenRelative(this Vector3 vector, Transform transform) {
            vector = transform.InverseTransformPoint(vector);
            vector.y = 0f;
            return transform.TransformPoint(vector);
        }

        public static Vector3 FlattenDirection(this Vector3 vector, Transform transform) {
            vector = transform.InverseTransformDirection(vector);
            vector.y = 0f;
            return transform.TransformDirection(vector);
        }

        // Flattens this vector except based on the up vector to keep direction
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
