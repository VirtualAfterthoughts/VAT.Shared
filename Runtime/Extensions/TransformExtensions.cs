using UnityEngine;

namespace VAT.Shared.Extensions
{
    /// <summary>
    /// Extension functions for Unity Transforms.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Transforms a rotation from local space to world space.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Quaternion TransformRotation(this Transform transform, Quaternion rotation) => transform.rotation * rotation;

        /// <summary>
        /// Transforms a rotation from world space to local space.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Quaternion InverseTransformRotation(this Transform transform, Quaternion rotation) => Quaternion.Inverse(transform.rotation) * rotation;
    }
}

