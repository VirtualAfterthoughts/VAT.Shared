using System;

using static Unity.Mathematics.math;

using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Data
{
    using Unity.Mathematics;

    /// <summary>
    /// A simplified transformation matrix providing position and rotation.
    /// </summary>
    [Serializable]
    public struct SimpleTransform
    {
        /// <summary>
        /// A SimpleTransform with no transformations.
        /// </summary>
        public static readonly SimpleTransform Identity = new(Vector3.zero, Quaternion.identity);

        /// <summary>
        /// The position of the transform.
        /// </summary>
        public float3 Position;

        /// <summary>
        /// The rotation of the transform.
        /// </summary>
        public quaternion Rotation;

        /// <summary>
        /// Matrix that transforms from local space into world space.
        /// </summary>
        public readonly Matrix4x4 LocalToWorldMatrix => Matrix4x4.TRS(Position, Rotation, Vector3.one);

        /// <summary>
        /// The forward direction of the rotation.
        /// </summary>
        public readonly float3 Forward => math.mul(Rotation, math.forward());

        /// <summary>
        /// The up direction of the rotation.
        /// </summary>
        public readonly float3 Up => math.mul(Rotation, math.up());

        /// <summary>
        /// The right direction of the rotation.
        /// </summary>
        public readonly float3 Right => math.mul(Rotation, math.right());

        /// <summary>
        /// The inverse of this SimpleTransform.
        /// </summary>
        public readonly SimpleTransform Inverse => new(-Position, inverse(Rotation));

        public SimpleTransform(float3 position) : this(position, quaternion.identity) { }

        public SimpleTransform(float3 position, quaternion rotation)
        {
            Position = position;
            Rotation = normalize(rotation);
        }

        /// <summary>
        /// Transforms a SimpleTransform from local space to world space.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public readonly SimpleTransform Transform(SimpleTransform transform)
        {
            return new(TransformPoint(transform.Position), TransformRotation(transform.Rotation));
        }

        /// <summary>
        /// Transforms a position from local space to world space.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public readonly float3 TransformPoint(float3 position)
        {
            BurstTransformExtensions.TransformPoint(position, this.Position, Rotation, out var result);
            return result;
        }

        /// <summary>
        /// Transforms a direction from local space to world space.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public readonly float3 TransformDirection(float3 direction)
        {
            BurstTransformExtensions.TransformDirection(direction, Rotation, out var result);
            return result;
        }

        /// <summary>
        /// Transforms a rotation from local space to world space.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public readonly quaternion TransformRotation(quaternion rotation)
        {
            BurstTransformExtensions.TransformRotation(rotation, this.Rotation, out var result);
            return result;
        }

        /// <summary>
        /// Transforms a SimpleTransform from world space to local space.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public readonly SimpleTransform InverseTransform(SimpleTransform transform)
        {
            return new(InverseTransformPoint(transform.Position), InverseTransformRotation(transform.Rotation));
        }

        /// <summary>
        /// Transforms a position from world space to local space.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public readonly float3 InverseTransformPoint(float3 position)
        {
            BurstTransformExtensions.InverseTransformPoint(position, this.Position, Rotation, out var result);
            return result;
        }

        /// <summary>
        /// Transforms a direction from world space to local space.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public readonly float3 InverseTransformDirection(float3 direction)
        {
            BurstTransformExtensions.InverseTransformDirection(direction, Rotation, out var result);
            return result;
        }

        /// <summary>
        /// Transforms a rotation from world space to local space.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public readonly quaternion InverseTransformRotation(quaternion rotation)
        {
            BurstTransformExtensions.InverseTransformRotation(rotation, this.Rotation, out var result);
            return result;
        }

        /// <summary>
        /// Linearly interpolates between a and b by t.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static SimpleTransform Lerp(SimpleTransform a, SimpleTransform b, float t)
        {
            return new(
                lerp(a.Position, b.Position, t),
                slerp(a.Rotation, b.Rotation, t)
            );
        }
    }
}
