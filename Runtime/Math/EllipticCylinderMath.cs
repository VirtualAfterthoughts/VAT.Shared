using UnityEngine;

using VAT.Shared.Data;

namespace VAT.Shared.Math
{
    /// <summary>
    /// Functions for calculating values of elliptic cylinders.
    /// </summary>
    public static class EllipticCylinderMath
    {
        public static float GetVolume(this EllipticCylinder ellipsoid) => GetVolume(ellipsoid.Ellipse.Radius, ellipsoid.Height);

        /// <summary>
        /// Gets the volume of an elliptic cylinder with the given dimensions.
        /// </summary>
        /// <param name="dimensions"></param>
        /// <returns></returns>
        public static float GetVolume(Vector3 dimensions) => GetVolume(dimensions.x, dimensions.y, dimensions.z);

        /// <summary>
        /// Gets the volume of an elliptic cylinder with the given radius and height.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static float GetVolume(Vector2 radius, float height) => GetVolume(radius.x, radius.y, height);

        /// <summary>
        /// Gets the volume of an elliptic cylinder with the given a and b dimensions and height.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static float GetVolume(float a, float b, float height) => EllipseMath.GetArea(a, b) * height;
    }
}
