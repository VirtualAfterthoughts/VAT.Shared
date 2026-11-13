#if USE_BURST
using Unity.Burst;

using static Unity.Mathematics.math;

namespace VAT.Shared.Math
{
    using Unity.Mathematics;

    /// <summary>
    /// Functions for operating on Unity.Mathematics quaternions.
    /// </summary>
    [BurstCompile(FloatMode = FloatMode.Fast)]
    public static class BurstQuaternionMath
    {
        /// <summary>
        /// Gets the axis and angle, in degrees, out of a quaternion.
        /// </summary>
        /// <param name="quaternion"></param>
        /// <param name="angle"></param>
        /// <param name="axis"></param>
        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void ToAngleAxis(in quaternion quaternion, out float angle, out float3 axis)
        {
            ToAxisAngle(quaternion, out axis, out angle);
            angle = degrees(angle);
        }

        /// <summary>
        /// Gets the axis and angle, in radians, out of a quaternion.
        /// </summary>
        /// <param name="quaternion"></param>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void ToAxisAngle(in quaternion quaternion, out float3 axis, out float angle)
        {
            var rot = quaternion;

            if (abs(rot.value.w) > 1.0f)
            {
                rot = normalize(rot);
            }

            angle = 2.0f * acos(rot.value.w);

            float den = (float)sqrt(1.0 - rot.value.w * rot.value.w);

            if (den > 0.0001f)
            {
                axis = rot.value.xyz / den;
            }
            else
            {
                axis = right();
            }
        }

        /// <summary>
        /// Returns the quaternion modified so that it is guaranteed to be the short way around.
        /// </summary>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static quaternion Shortest(this quaternion quaternion)
        {
            if (quaternion.value.w < 0)
            {
                return new quaternion(-quaternion.value.x, -quaternion.value.y, -quaternion.value.z, -quaternion.value.w);
            }

            return quaternion;
        }
    }
}
#endif