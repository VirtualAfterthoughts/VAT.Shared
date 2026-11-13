#if USE_BURST
using Unity.Burst;

using static Unity.Mathematics.math;

namespace VAT.Shared.Math
{
    using Unity.Mathematics;

    /// <summary>
    /// Burst equivalents of <see cref="JointMath"/>.
    /// </summary>
    [BurstCompile]
    public static class BurstJointMath
    {
        /// <summary>
        /// Gets the joint's space conversion rotation for calculating target positions and rotations.
        /// </summary>
        /// <param name="initialRotation">The joint's initial rotation, stored from when the joint is first created.</param>
        /// <param name="axis">The joint's axis.</param>
        /// <param name="secondaryAxis">The joint's secondary axis.</param>
        /// <param name="jointRotation">The resulting joint rotation.</param>
        /// <returns></returns>
        [BurstCompile]
        public static void GetJointRotation(in quaternion initialRotation, in float3 axis, in float3 secondaryAxis, out quaternion jointRotation)
        {
            float3 forward = cross(axis, secondaryAxis);
            float3 up = -cross(axis, forward);

            jointRotation = mul(initialRotation, quaternion.LookRotation(forward, up));
        }

        /// <summary>
        /// Gets a joint target rotation from a world-space rotation given initial conditions.
        /// </summary>
        /// <param name="jointRotation">The joint's solving rotation, calculated from <see cref="GetJointRotation(quaternion, float3, float3)"/> when the joint is first created.</param>
        /// <param name="initialRotation">The joint's initial rotation, stored from when the joint is first created.</param>
        /// <param name="targetRotation">The target rotation in world space.</param>
        /// <param name="initialConnectedRotation">The joint connected body's initial rotation, stored from when the joint is first created.</param>
        /// <param name="connectedRotation">The joint connected body's current rotation.</param>
        /// <param name="result">The resulting target rotation.</param>
        /// <returns></returns>
        [BurstCompile]
        public static void GetTargetRotationWorld(in quaternion jointRotation, in quaternion initialRotation, in quaternion targetRotation, in quaternion initialConnectedRotation, in quaternion connectedRotation, out quaternion result)
        {
            result = inverse(jointRotation);
            result = mul(result, mul(initialRotation, inverse(targetRotation)));
            result = mul(result, inverse(mul(initialConnectedRotation, inverse(connectedRotation))));
            result = mul(result, jointRotation);
        }

        /// <summary>
        /// Gets a joint target rotation from a world-space rotation given initial conditions.
        /// </summary>
        /// <param name="jointRotation">The joint's solving rotation, calculated from <see cref="GetJointRotation(quaternion, float3, float3)"/> when the joint is first created.</param>
        /// <param name="initialRotation">The joint's initial rotation, stored from when the joint is first created.</param>
        /// <param name="targetRotation">The target rotation in world space.</param>
        /// <param name="result">The resulting target rotation.</param>
        /// <returns></returns>
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