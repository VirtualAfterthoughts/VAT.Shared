using UnityEngine;

using static Unity.Mathematics.math;

using Unity.Burst;

using VAT.Shared.Extensions;

namespace VAT.Shared.Math
{
    using Unity.Mathematics;

    public static class Derivatives
    {
        public static float3 GetLinearDisplacement(float3 from, float3 to) => to - from;

        public static float3 GetAngularDisplacement(quaternion from, quaternion to)
        {
            // We get the displacement between the quaternions, normalize it to ensure there are no math errors
            // Finally, we check the w component to make sure it is the shortest possible rotation
            quaternion q = normalize(mul(to, inverse(from))).Shortest();

            // Now we just have to convert the rotation to an angle and axis
            q.ToAxisAngle(out float3 x, out float xMag);
            x = normalize(x);
            x *= xMag;
            return x;
        }

        public static float3 GetLinearVelocity(float3 from, float3 to) => GetLinearVelocity(from, to, Time.deltaTime);

        public static float3 GetLinearVelocity(float3 from, float3 to, float deltaTime) => GetLinearDisplacement(from, to) / deltaTime;

        public static float3 GetAngularVelocity(quaternion from, quaternion to) => GetAngularVelocity(from, to, Time.deltaTime);

        public static float3 GetAngularVelocity(quaternion from, quaternion to, float deltaTime) => GetAngularDisplacement(from, to) / deltaTime;

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

    [BurstCompile(FloatMode = FloatMode.Fast)]
    public static partial class BurstDerivatives
    {
        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void GetLinearDisplacement(in float3 from, in float3 to, out float3 linearDisplacement)
        {
            linearDisplacement = to - from;
        }

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void GetLinearVelocity(in float3 from, in float3 to, in float delta, out float3 linearVelocity)
        {
            GetLinearDisplacement(from, to, out linearVelocity);
            linearVelocity /= delta;
        }

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void GetAngularDisplacement(in quaternion from, in quaternion to, out float3 angularDisplacement)
        {
            // We get the displacement between the quaternions, normalize it to ensure there are no math errors
            // Finally, we check the w component to make sure it is the shortest possible rotation
            quaternion q = normalize(mul(to, inverse(from))).Shortest();

            // Now we just have to convert the rotation to an angle and axis
            BurstQuaternionExtensions.ToAxisAngle(q, out float3 x, out float xMag);
            angularDisplacement = normalize(x);
            angularDisplacement *= xMag;
        }

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void GetAngularVelocity(in quaternion from, in quaternion to, in float delta, out float3 angularVelocity)
        {
            GetAngularDisplacement(from, to, out angularVelocity);

            if (delta > 0f)
            {
                angularVelocity /= delta;
            }
        }
    }
}