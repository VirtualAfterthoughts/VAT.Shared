using UnityEngine;
using System.Collections.Generic;

using VAT.Shared.Extensions;
using VAT.Shared.Math;

namespace VAT.Shared.Extensions
{
    public static partial class PhysicsExtensions {
        /// <summary>
        /// Clamps the cylinder height within range of the radius.
        /// </summary>
        /// <param name="radius">The radius of the cylinder.</param>
        /// <param name="height">The height of the cylinder.</param>
        /// <returns></returns>
        public static float GetClampedHeight(float radius, float height) {
            return Mathf.Max(radius * 2f, height) * 0.5f;
        }


        /// <summary>
        /// Calculates the volume of a rectangular prism.
        /// Shortcut for v = l * w * h where l is length, w is width, and h is height.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static float GetRectangularPrismVolume(float x, float y, float z) => x * y * z;

        /// <summary>
        /// Calculates the volume of a rectangular prism.
        /// Shortcut for v = l * w * h where l is length, w is width, and h is height.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static float GetRectangularPrismVolume(Vector3 size) => GetRectangularPrismVolume(size.x, size.y, size.z);

        /// <summary>
        /// Calculates the volume of a box collider.
        /// Shortcut for v = l * w * h where l is length, w is width, and h is height.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        public static float GetVolume(this BoxCollider collider) => GetRectangularPrismVolume(Vector3.Scale(collider.size, collider.transform.lossyScale));

        /// <summary>
        /// Calculates the volume of a sphere.
        /// Shortcut for v = (4/3)?r^3 where r is the radius.
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static float GetSphereVolume(float radius) => 1.33333333333f * Mathf.PI * Mathf.Pow(radius, 3f);

        /// <summary>
        /// Calculates the volume of a sphere.
        /// Shortcut for v = (4/3)?r^3 where r is the radius.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        public static float GetVolume(this SphereCollider collider) => GetSphereVolume(collider.radius * collider.transform.lossyScale.Maximum());

        /// <summary>
        /// Calculates the volume of a capsule.
        /// Shortcut for v = ?r^2((4/3)r + a) where r is radius and a is its side length.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="sideLength"></param>
        /// <returns></returns>
        public static float GetCapsuleVolume(float radius, float sideLength) => Mathf.PI * radius * radius * (1.33333333333f * radius + sideLength);

        /// <summary>
        /// Calculates the volume of a capsule.
        /// Shortcut for v = ?r^2((4/3)r + a) where r is radius and a is its side length.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        public static float GetVolume(this CapsuleCollider collider) {
            var targetTransform = collider.transform;
            var scale = targetTransform.lossyScale;

            scale[collider.direction] = 0f;
            float height = collider.height * targetTransform.lossyScale[collider.direction];
            float worldRadius = collider.radius * scale.Maximum();
            float worldHeight = GetClampedHeight(worldRadius, height);
            return GetCapsuleVolume(worldRadius, worldHeight);
        }

    }
}