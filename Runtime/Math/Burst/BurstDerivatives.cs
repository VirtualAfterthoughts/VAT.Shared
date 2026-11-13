#if USE_BURST
using static Unity.Mathematics.math;

using Unity.Burst;

namespace VAT.Shared.Math
{
    using Unity.Mathematics;

    /// <summary>
    /// Burst equivalents of <see cref="Derivatives"/>. These are set to compile with <see cref="FloatMode.Fast"/>.
    /// </summary>
    [BurstCompile(FloatMode = FloatMode.Fast)]
    public static partial class BurstDerivatives
    {
        /// <summary>
        /// Gets the linear displacement between two points.
        /// </summary>
        /// <param name="from">The start point.</param>
        /// <param name="to">The end point.</param>
        /// <param name="linearDisplacement">The resulting linear displacement.</param>
        /// <returns></returns>
        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void GetLinearDisplacement(in float3 from, in float3 to, out float3 linearDisplacement) => linearDisplacement = to - from;

        /// <summary>
        /// Gets the linear velocity between two points over a given delta time.
        /// </summary>
        /// <param name="from">The start point.</param>
        /// <param name="to">The end point.</param>
        /// <param name="deltaTime">The change in time that the translation occurred over.</param>
        /// <param name="linearVelocity">The resulting linear velocity.</param>
        /// <returns></returns>
        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void GetLinearVelocity(in float3 from, in float3 to, in float deltaTime, out float3 linearVelocity)
        {
            GetLinearDisplacement(from, to, out var linearDisplacement);
            linearVelocity = linearDisplacement / deltaTime;
        }

        /// <summary>
        /// Gets the angular displacement between two rotations.
        /// </summary>
        /// <param name="from">The start rotation.</param>
        /// <param name="to">The end rotation.</param>
        /// <param name="angularDisplacement">The resulting angular displacement.</param>
        /// <returns></returns>
        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void GetAngularDisplacement(in quaternion from, in quaternion to, out float3 angularDisplacement)
        {
            // We get the displacement between the quaternions, normalize it to ensure there are no math errors
            // Finally, we check the w component to make sure it is the shortest possible rotation
            quaternion q = normalize(mul(to, inverse(from))).Shortest();

            // Now we just have to convert the rotation to an angle and axis
            BurstQuaternionMath.ToAxisAngle(q, out float3 x, out float xMag);
            angularDisplacement = normalize(x);
            angularDisplacement *= xMag;
        }

        /// <summary>
        /// Gets the angular velocity between two rotations over a given delta time.
        /// </summary>
        /// <param name="from">The start rotation.</param>
        /// <param name="to">The end rotation.</param>
        /// <param name="deltaTime">The change in time that the rotation occurred over.</param>
        /// <param name="angularVelocity">The resulting angular velocity.</param>
        /// <returns></returns>
        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void GetAngularVelocity(in quaternion from, in quaternion to, in float deltaTime, out float3 angularVelocity)
        {
            GetAngularDisplacement(from, to, out angularVelocity);

            if (deltaTime > 0f)
            {
                angularVelocity /= deltaTime;
            }
        }
    }
}
#endif