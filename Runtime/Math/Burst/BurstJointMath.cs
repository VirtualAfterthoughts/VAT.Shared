#if USE_BURST
using Unity.Burst;

using static Unity.Mathematics.math;

namespace VAT.Shared.Math
{
    using Unity.Mathematics;

    [BurstCompile]
    public static class BurstJointMath
    {
        [BurstCompile]
        public static void GetJointRotation(in quaternion initialRotation, in float3 axis, in float3 secondaryAxis, out quaternion jointRotation)
        {
            float3 forward = cross(axis, secondaryAxis);
            float3 up = -cross(axis, forward);

            jointRotation = mul(initialRotation, quaternion.LookRotation(forward, up));
        }

        [BurstCompile]
        public static void GetTargetRotationWorld(in quaternion jointRotation, in quaternion initialRotation, in quaternion targetRotation, in quaternion initialConnectedRotation, in quaternion connectedRotation, out quaternion result)
        {
            result = inverse(jointRotation);
            result = mul(result, mul(initialRotation, inverse(targetRotation)));
            result = mul(result, inverse(mul(initialConnectedRotation, inverse(connectedRotation))));
            result = mul(result, jointRotation);
        }

        [BurstCompile]
        public static void GetTargetRotationWorld(in quaternion jointRotation, in quaternion initialRotation, in quaternion targetRotation, out quaternion result)
        {
            result = inverse(jointRotation);
            result = mul(result, mul(initialRotation, inverse(targetRotation)));
            result = mul(result, jointRotation);
        }
    }
}
#endif