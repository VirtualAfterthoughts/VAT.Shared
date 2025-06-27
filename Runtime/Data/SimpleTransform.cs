using System;

using UnityEngine;

namespace VAT.Shared.Data
{
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
        public Vector3 Position;

        /// <summary>
        /// The rotation of the transform.
        /// </summary>
        public Quaternion Rotation;

        /// <summary>
        /// Matrix that transforms from local space into world space.
        /// </summary>
        public readonly Matrix4x4 LocalToWorldMatrix => Matrix4x4.TRS(Position, Rotation, Vector3.one);

        /// <summary>
        /// The forward direction of the rotation.
        /// </summary>
        public readonly Vector3 Forward => Rotation * Vector3.forward;

        /// <summary>
        /// The up direction of the rotation.
        /// </summary>
        public readonly Vector3 Up => Rotation * Vector3.up;

        /// <summary>
        /// The right direction of the rotation.
        /// </summary>
        public readonly Vector3 Right => Rotation * Vector3.right;

        /// <summary>
        /// The inverse of this SimpleTransform.
        /// </summary>
        public readonly SimpleTransform Inverse => new(-Position, Quaternion.Inverse(Rotation));

        public SimpleTransform(Vector3 position) : this(position, Quaternion.identity) { }

        public SimpleTransform(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
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
        public readonly Vector3 TransformPoint(Vector3 position) => (this.Rotation * position) + this.Position;

        /// <summary>
        /// Transforms a direction from local space to world space.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public readonly Vector3 TransformDirection(Vector3 direction) => this.Rotation * direction;

        /// <summary>
        /// Transforms a rotation from local space to world space.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public readonly Quaternion TransformRotation(Quaternion rotation) => this.Rotation * rotation;

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
        public readonly Vector3 InverseTransformPoint(Vector3 position) => Quaternion.Inverse(this.Rotation) * (position - this.Position);

        /// <summary>
        /// Transforms a direction from world space to local space.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public readonly Vector3 InverseTransformDirection(Vector3 direction) => Quaternion.Inverse(this.Rotation) * direction;

        /// <summary>
        /// Transforms a rotation from world space to local space.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public readonly Quaternion InverseTransformRotation(Quaternion rotation) => Quaternion.Inverse(this.Rotation) * rotation;

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
                Vector3.Lerp(a.Position, b.Position, t),
                Quaternion.Slerp(a.Rotation, b.Rotation, t)
            );
        }
    }
}
