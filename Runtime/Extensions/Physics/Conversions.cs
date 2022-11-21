using UnityEngine;

using static Unity.Mathematics.math;

using Unity.Burst;

namespace VAT.Shared.Extensions {
    using Unity.Mathematics;

    public static partial class PhysicsExtensions {
        /// <summary>
        /// Returns the displacement between two vectors.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float3 GetLinearDisplacement(float3 from, float3 to) => to - from;

        /// <summary>
        /// Returns the displacement between two rotations.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float3 GetAngularDisplacement(quaternion from, quaternion to) {
            // We get the displacement between the quaternions, normalize it to ensure there are no math errors
            // Finally, we check the w component to make sure it is the shortest possible rotation
            quaternion q = normalize(mul(to, inverse(from))).shortest();

            // Now we just have to convert the rotation to an angle and axis
            q.toaxisangle(out float3 x, out float xMag);
            x = normalize(x);
            x *= xMag;
            return x;
        }

        /// <summary>
        /// Returns the linear velocity between two points in the last frame.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float3 GetLinearVelocity(float3 from, float3 to) => GetLinearDisplacement(from, to) / Time.deltaTime;
        
        /// <summary>
        /// Returns the linear velocity between two points with a given time delta.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static float3 GetLinearVelocity(float3 from, float3 to, float delta) => GetLinearDisplacement(from, to) / delta;

        /// <summary>
        /// Returns the angular velocity between two rotations in the last frame.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float3 GetAngularVelocity(quaternion from, quaternion to) => GetAngularDisplacement(from, to) / Time.deltaTime;

        /// <summary>
        /// Returns the angular velocity between two rotations with a given time delta.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static float3 GetAngularVelocity(quaternion from, quaternion to, float delta) => GetAngularDisplacement(from, to) / delta;

        /// <summary>
        /// Gets the to position based on velocity and the from position.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="velocity"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static Vector3 GetToPosition(Vector3 from, Vector3 velocity, float delta) => from + (velocity * delta);

        /// <summary>
        /// Gets the to position based on velocity and the from position.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="velocity"></param>
        /// <returns></returns>
        public static Vector3 GetToPosition(Vector3 from, Vector3 velocity) => GetToPosition(from, velocity, Time.deltaTime);

        /// <summary>
        /// Gets the from position based on velocity and the to position.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="velocity"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static Vector3 GetFromPosition(Vector3 to, Vector3 velocity, float delta) => to - (velocity * delta);

        /// <summary>
        /// Gets the from position based on velocity and the to position.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="velocity"></param>
        /// <returns></returns>
        public static Vector3 GetFromPosition(Vector3 to, Vector3 velocity) => GetFromPosition(to, velocity, Time.deltaTime);

        /// <summary>
        /// Reverses a GetAngularDisplacement operation to only return the initial displacement value.
        /// </summary>
        /// <param name="displacement"></param>
        /// <returns></returns>
        public static Quaternion GetQuaternionDisplacement(Vector3 displacement) {
            float xMag = displacement.magnitude * Mathf.Rad2Deg;
            Vector3 x = displacement.normalized;

            return Quaternion.AngleAxis(xMag, x);
        }

        /// <summary>
        /// Gets the to rotation based on angular velocity and the from rotation.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="angularVelocity"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static Quaternion GetToRotation(Quaternion from, Vector3 angularVelocity, float delta) => GetQuaternionDisplacement(angularVelocity * delta) * from;

        /// <summary>
        /// Gets the to rotation based on angular velocity and the from rotation.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="angularVelocity"></param>
        /// <returns></returns>
        public static Quaternion GetToRotation(Quaternion from, Vector3 angularVelocity) => GetToRotation(from, angularVelocity, Time.deltaTime);

        /// <summary>
        /// Gets the from rotation based on angular velocity and the to rotation.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="angularVelocity"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static Quaternion GetFromRotation(Quaternion to, Vector3 angularVelocity, float delta) => Quaternion.Inverse(GetQuaternionDisplacement(angularVelocity * delta)) * to;

        /// <summary>
        /// Gets the from rotation based on angular velocity and the to rotation.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="angularVelocity"></param>
        /// <returns></returns>
        public static Quaternion GetFromRotation(Quaternion to, Vector3 angularVelocity) => GetFromRotation(to, angularVelocity, Time.deltaTime);
    }

    [BurstCompile(FloatMode = FloatMode.Fast)]
    public static partial class BurstCompiled_PhysicsExtensions {
        /// <summary>
        /// Returns the displacement between two vectors (BURST).
        /// </summary>
        /// <param name="from">The previous position.</param>
        /// <param name="to">The current position.</param>
        /// <param name="result">The output vector.</param>
        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void BurstCompiled_GetLinearDisplacement(in float3 from, in float3 to, out float3 result) {
            result = to - from;
        }

        /// <summary>
        /// Returns the linear velocity between two points in the last frame (BURST).
        /// </summary>
        /// <param name="from">The previous position.</param>
        /// <param name="to">The current position.</param>
        /// <param name="delta">The time passed between the two positions.</param>
        /// <param name="result">The output velocity.</param>
        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void BurstCompiled_GetLinearVelocity(in float3 from, in float3 to, in float delta, out float3 result) {
            BurstCompiled_GetLinearDisplacement(from, to, out result);
            result /= delta;
        }

        /// <summary>
        /// Returns the displacement between two rotations (BURST).
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void BurstCompiled_GetAngularDisplacement(in quaternion from, in quaternion to, out float3 result)
        {
            // We get the displacement between the quaternions, normalize it to ensure there are no math errors
            // Finally, we check the w component to make sure it is the shortest possible rotation
            quaternion q = normalize(mul(to, inverse(from))).shortest();

            // Now we just have to convert the rotation to an angle and axis
            BurstCompiled_QuaternionExtensions.BurstCompiled_toaxisangle(q, out float3 x, out float xMag);
            result = normalize(x);
            result *= xMag;
        }

        /// <summary>
        /// Returns the angular velocity between two rotations with a given time delta (BURST).
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void BurstCompiled_GetAngularVelocity(in quaternion from, in quaternion to, in float delta, out float3 result)
        {
            // Get basic direction
            BurstCompiled_GetAngularDisplacement(from, to, out result);

            // Div by zero check
            if (delta > 0f)
                result /= delta;

            // NaN check
            if (isnan(result).istrue())
                result = float3.zero;
        }
    }
}