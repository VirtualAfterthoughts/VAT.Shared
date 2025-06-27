using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Math
{
    public static class Derivatives
    {
        public static Vector3 GetLinearDisplacement(Vector3 from, Vector3 to) => to - from;

        public static Vector3 GetAngularDisplacement(Quaternion from, Quaternion to)
        {
            // We get the displacement between the quaternions, normalize it to ensure there are no math errors
            // Finally, we check the w component to make sure it is the shortest possible rotation
            Quaternion q = to * Quaternion.Inverse(from).Shortest();

            // Now we just have to convert the rotation to an angle and axis
            q.ToAngleAxis(out var angle, out var axis);
            float magnitude = angle * Mathf.Deg2Rad;

            return axis.normalized * magnitude;
        }

        public static Vector3 GetLinearVelocity(Vector3 from, Vector3 to) => GetLinearVelocity(from, to, Time.deltaTime);

        public static Vector3 GetLinearVelocity(Vector3 from, Vector3 to, float deltaTime) => GetLinearDisplacement(from, to) / deltaTime;

        public static Vector3 GetAngularVelocity(Quaternion from, Quaternion to) => GetAngularVelocity(from, to, Time.deltaTime);

        public static Vector3 GetAngularVelocity(Quaternion from, Quaternion to, float deltaTime) => GetAngularDisplacement(from, to) / deltaTime;

        public static Vector3 GetNextPosition(Vector3 from, Vector3 linearVelocity, float deltaTime) => from + (linearVelocity * deltaTime);

        public static Vector3 GetNextPosition(Vector3 from, Vector3 linearVelocity) => GetNextPosition(from, linearVelocity, Time.deltaTime);

        public static Quaternion GetQuaternionDisplacement(Vector3 angularDisplacement)
        {
            float xMag = angularDisplacement.magnitude * Mathf.Rad2Deg;

            Vector3 x = angularDisplacement.normalized;

            return Quaternion.AngleAxis(xMag, x);
        }

        public static Quaternion GetNextRotation(Quaternion from, Vector3 angularVelocity, float deltaTime) => GetQuaternionDisplacement(angularVelocity * deltaTime) * from;

        public static Quaternion GetNextRotation(Quaternion from, Vector3 angularVelocity) => GetNextRotation(from, angularVelocity, Time.deltaTime);
    }
}