using UnityEngine;

using static Unity.Mathematics.math;

namespace VAT.Shared.Extensions
{
    using Unity.Burst;
    using Unity.Mathematics;

    public static class TransformExtensions
    {
        /// <summary>
        /// Transforms rotation from local space to world space.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Quaternion TransformRotation(this Transform transform, Quaternion rotation) => transform.rotation * rotation;

        /// <summary>
        /// Transforms rotation from world space to local space.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Quaternion InverseTransformRotation(this Transform transform, Quaternion rotation) => Quaternion.Inverse(transform.rotation) * rotation;
    }

    [BurstCompile(FloatMode = FloatMode.Fast)]
    public static class BurstTransformExtensions
    {
        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void TransformPoint(in float3 input, in float3 position, in quaternion rotation, in float3 lossyScale, out float3 result) => result = mul(rotation, input * lossyScale) + position;

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void TransformDirection(in float3 input, in quaternion rotation, out float3 result) => result = mul(rotation, input);

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void TransformVector(in float3 input, in quaternion rotation, in float3 lossyScale, out float3 result) => TransformDirection(input * lossyScale, rotation, out result);

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void TransformRotation(in quaternion input, in quaternion rotation, in float3 lossyScale, out quaternion result)
        {
            result = mul(rotation, input);
        }

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void InverseTransformPoint(in float3 input, in float3 position, in quaternion rotation, in float3 lossyScale, out float3 result) => result = mul(inverse(rotation), input - position) / lossyScale;

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void InverseTransformDirection(in float3 input, in quaternion rotation, out float3 result) => result = mul(inverse(rotation), input);

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void InverseTransformVector(in float3 input, in quaternion rotation, in float3 lossyScale, out float3 result)
        {
            InverseTransformDirection(input, rotation, out float3 vector);
            result = vector / lossyScale;
        }

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void InverseTransformRotation(in quaternion input, in quaternion rotation, in float3 lossyScale, out quaternion result)
        {
            result = mul(inverse(rotation), input);
        }
    }
}

