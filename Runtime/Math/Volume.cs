using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Math
{
    public static class Volume
    {
        public static float GetClampedHeight(float radius, float height)
        {
            return Mathf.Max(radius * 2f, height) * 0.5f;
        }

        public static float GetRectangularPrismVolume(float x, float y, float z)
        {
            return x * y * z;
        }

        public static float GetRectangularPrismVolume(Vector3 size) => GetRectangularPrismVolume(size.x, size.y, size.z);

        public static float GetVolume(this BoxCollider collider) => GetRectangularPrismVolume(Vector3.Scale(collider.size, collider.transform.lossyScale));

        public static float GetSphereVolume(float radius) => 4f/3f * Mathf.PI * Mathf.Pow(radius, 3f);

        public static float GetVolume(this SphereCollider collider) => GetSphereVolume(collider.radius * collider.transform.lossyScale.Max());

        public static float GetCapsuleVolume(float radius, float height) => Mathf.PI * radius * radius * (4f/3f * radius + height);

        public static float GetVolume(this CapsuleCollider collider)
        {
            var targetTransform = collider.transform;
            var scale = targetTransform.lossyScale;

            scale[collider.direction] = 0f;

            float height = collider.height * targetTransform.lossyScale[collider.direction];

            float worldRadius = collider.radius * scale.Max();
            float worldHeight = GetClampedHeight(worldRadius, height);

            return GetCapsuleVolume(worldRadius, worldHeight);
        }
    }
}