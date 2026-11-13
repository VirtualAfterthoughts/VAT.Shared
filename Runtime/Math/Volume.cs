using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Math
{
    /// <summary>
    /// Functions for calculating the volume of different shapes.
    /// </summary>
    public static class Volume
    {
        /// <summary>
        /// Clamps a height with a given radius for a capsule.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static float ClampCapsuleHeight(float radius, float height) => Mathf.Max(radius * 2f, height) * 0.5f;

        /// <summary>
        /// Gets the volume of a rectangular prism.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static float GetRectangularPrismVolume(float x, float y, float z) => x * y * z;

        /// <summary>
        /// Gets the volume of a rectangular prism.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static float GetRectangularPrismVolume(Vector3 size) => GetRectangularPrismVolume(size.x, size.y, size.z);

        /// <summary>
        /// Gets the volume of a BoxCollider.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        public static float GetVolume(this BoxCollider collider) => GetRectangularPrismVolume(Vector3.Scale(collider.size, collider.transform.lossyScale));

        /// <summary>
        /// Gets the volume of a sphere.
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static float GetSphereVolume(float radius) => 4f / 3f * Mathf.PI * Mathf.Pow(radius, 3f);

        /// <summary>
        /// Gets the volume of a SphereCollider.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        public static float GetVolume(this SphereCollider collider) => GetSphereVolume(collider.radius * collider.transform.lossyScale.Max());

        /// <summary>
        /// Gets the volume of a capsule.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static float GetCapsuleVolume(float radius, float height) => Mathf.PI * radius * radius * (4f / 3f * radius + height);

        /// <summary>
        /// Gets the volume of a CapsuleCollider.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        public static float GetVolume(this CapsuleCollider collider)
        {
            var targetTransform = collider.transform;
            var scale = targetTransform.lossyScale;

            scale[collider.direction] = 0f;

            float height = collider.height * targetTransform.lossyScale[collider.direction];

            float worldRadius = collider.radius * scale.Max();
            float worldHeight = ClampCapsuleHeight(worldRadius, height);

            return GetCapsuleVolume(worldRadius, worldHeight);
        }
    }
}