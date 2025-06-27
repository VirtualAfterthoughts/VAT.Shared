using UnityEngine;

namespace VAT.Shared.Extensions
{
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
}

