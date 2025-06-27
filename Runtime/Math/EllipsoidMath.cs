using UnityEngine;

using VAT.Shared.Data;

namespace VAT.Shared.Math
{
    public static class EllipsoidMath
    {
        public static float GetVolume(this Ellipsoid ellipsoid) => GetVolume(ellipsoid.Ellipse.Radius, ellipsoid.Height);

        public static float GetVolume(Vector3 dimensions) => GetVolume(dimensions.x, dimensions.y, dimensions.z);

        public static float GetVolume(Vector2 radius, float height) => GetVolume(radius.x, radius.y, height);

        public static float GetVolume(float a, float b, float height) => EllipseMath.GetArea(a, b) * height;
    }
}
