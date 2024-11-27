using UnityEngine;
using Unity.Burst;

using static Unity.Mathematics.math;

namespace VAT.Shared.Extensions
{
    using Unity.Mathematics;

    public static class QuaternionExtensions
    {
        public static Quaternion Shortest(this Quaternion quat)
        {
            if (quat.w < 0f)
            {
                return new Quaternion(-quat.x, -quat.y, -quat.z, -quat.w);
            }

            return quat;
        }

        public static quaternion Shortest(this quaternion quat)
        {
            if (quat.value.w < 0)
            {
                return new quaternion(-quat.value.x, -quat.value.y, -quat.value.z, -quat.value.w);
            }

            return quat;
        }

        public static void ToAxisAngle(this quaternion quat, out float3 axis, out float angle)
        {
            BurstQuaternionExtensions.ToAxisAngle(quat, out axis, out angle);
        }
    }

    [BurstCompile(FloatMode = FloatMode.Fast)]
    public static class BurstQuaternionExtensions
    {
        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void ToAngleAxis(in quaternion quat, out float angle, out float3 axis)
        {
            ToAxisAngle(quat, out axis, out angle);
            angle = degrees(angle);
        }

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void ToAxisAngle(in quaternion quat, out float3 axis, out float angle)
        {
            var rot = quat;

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
    }
}
