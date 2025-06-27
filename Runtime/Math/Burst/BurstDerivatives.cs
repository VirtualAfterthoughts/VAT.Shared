#if USE_BURST
using static Unity.Mathematics.math;

using Unity.Burst;

using VAT.Shared.Extensions;

namespace VAT.Shared.Math
{
    using Unity.Mathematics;

    [BurstCompile(FloatMode = FloatMode.Fast)]
    public static partial class BurstDerivatives
    {
        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void GetLinearDisplacement(in float3 from, in float3 to, out float3 linearDisplacement) => linearDisplacement = to - from;

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void GetLinearVelocity(in float3 from, in float3 to, in float deltaTime, out float3 linearVelocity)
        {
            GetLinearDisplacement(from, to, out var linearDisplacement);
            linearVelocity = linearDisplacement / deltaTime;
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