using System;

using static Unity.Mathematics.math;

using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Data
{
    using Unity.Mathematics;

    /// <summary>
    /// A transform in data form.
    /// </summary>
    [Serializable]
    public struct SimpleTransform
    {
        /// <summary>
        /// A SimpleTransform with default values.
        /// </summary>
        public static readonly SimpleTransform Default = Create(float3.zero, quaternion.identity);

        /// <summary>
        /// The position of the transform.
        /// </summary>
        public float3 position;

        /// <summary>
        /// The rotation of the transform.
        /// </summary>
        public quaternion rotation;

        /// <summary>
        /// The scale of the transform.
        /// </summary>
        public float3 scale;

        /// <summary>
        /// Matrix that transforms a point from local space into world space.
        /// </summary>
        public Matrix4x4 localToWorldMatrix
        {
            get
            {
                return Matrix4x4.TRS(position, rotation, scale);
            }
        }

        /// <summary>
        /// The forward vector of the snapshot.
        /// </summary>
        public float3 forward => math.mul(rotation, math.forward());

        /// <summary>
        /// The up vector of the snapshot.
        /// </summary>
        public float3 up => math.mul(rotation, math.up());

        /// <summary>
        /// The right vector of the snapshot.
        /// </summary>
        public float3 right => math.mul(rotation, math.right());

        /// <summary>
        /// Creates an inverse of this transform's position and rotation.
        /// </summary>
        /// <param name="transform">The original transform.</param>
        /// <returns></returns>
        public static SimpleTransform Inverse(SimpleTransform transform)
        {
            return Create(-transform.position, inverse(transform.rotation), transform.scale);
        }

        /// <summary>
        /// Creates a snapshot of this position and rotation with a scale of (1, 1, 1).
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static SimpleTransform Create(float3 position, quaternion rotation)
        {
            return Create(position, rotation, 1f);
        }

        /// <summary>
        /// Creates a snapshot of this position, rotation, and scale.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static SimpleTransform Create(float3 position, quaternion rotation, float3 scale)
        {
            SimpleTransform simple;
            simple.position = position;
            simple.rotation = normalize(rotation);
            simple.scale = scale;
            return simple;
        }

        /// <summary>
        /// Transforms a SimpleTransform from local space to world space.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public SimpleTransform Transform(SimpleTransform transform)
        {
            return Create(TransformPoint(transform.position), TransformRotation(transform.rotation), transform.scale * scale);
        }

        /// <summary>
        /// Transforms position from local space to world space.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float3 TransformPoint(float3 position)
        {
            BurstTransformExtensions.TransformPoint(position, this.position, rotation, scale, out var result);
            return result;
        }

        /// <summary>
        /// Transforms direction from local space to world space.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public float3 TransformDirection(float3 direction)
        {
            BurstTransformExtensions.TransformDirection(direction, rotation, out var result);
            return result;
        }

        /// <summary>
        /// Transforms vector from local space to world space.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public float3 TransformVector(float3 vector)
        {
            BurstTransformExtensions.TransformVector(vector, rotation, scale, out var result);
            return result;
        }

        /// <summary>
        /// Transforms rotation from local space to world space.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public quaternion TransformRotation(quaternion rotation)
        {
            BurstTransformExtensions.TransformRotation(rotation, this.rotation, this.scale, out var result);
            return result;
        }

        /// <summary>
        /// Transforms a SimpleTransform from world space to local space.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public SimpleTransform InverseTransform(SimpleTransform transform)
        {
            return Create(InverseTransformPoint(transform.position), InverseTransformRotation(transform.rotation), transform.scale / scale);
        }

        /// <summary>
        /// Transforms position from world space to local space.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float3 InverseTransformPoint(float3 position)
        {
            BurstTransformExtensions.InverseTransformPoint(position, this.position, rotation, scale, out var result);
            return result;
        }

        /// <summary>
        /// Transforms direction from world space to local space.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public float3 InverseTransformDirection(float3 direction)
        {
            BurstTransformExtensions.InverseTransformDirection(direction, rotation, out var result);
            return result;
        }

        /// <summary>
        /// Transforms vector from world space to local space.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public float3 InverseTransformVector(float3 vector)
        {
            BurstTransformExtensions.InverseTransformVector(vector, rotation, scale, out var result);
            return result;
        }

        /// <summary>
        /// Transforms rotation from world space to local space.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public quaternion InverseTransformRotation(quaternion rotation)
        {
            BurstTransformExtensions.InverseTransformRotation(rotation, this.rotation, scale, out var result);
            return result;
        }

        /// <summary>
        /// Creates a SimpleTransform that is t percent between a and b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static SimpleTransform Lerp(SimpleTransform a, SimpleTransform b, float t)
        {
            return Create(
                lerp(a.position, b.position, t),
                slerp(a.rotation, b.rotation, t),
                lerp(a.scale, b.scale, t)
            );
        }
    }
}
