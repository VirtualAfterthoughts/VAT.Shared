using UnityEngine;

namespace VAT.Shared.Data
{
    using System;

    public static class EllipseExtensions
    {
        /// <summary>
        /// Converts an ellipse to another ellipse type.
        /// </summary>
        /// <typeparam name="TEllipse"></typeparam>
        /// <returns></returns>
        public static TEllipse Convert<TEllipse>(this IEllipse original) where TEllipse : IEllipse
        {
            var instance = Activator.CreateInstance<TEllipse>();
            instance.SetRadius(original.GetRadius());
            return instance;
        }
    }

    public interface IEllipse
    {
        /// <summary>
        /// Converts this ellipse to its interface form.
        /// </summary>
        /// <returns></returns>
        public IEllipse AsInterface();

        /// <summary>
        /// Sets the radius of the ellipse.
        /// </summary>
        /// <param name="radius">The new radius.</param>
        public void SetRadius(Vector2 radius);

        /// <summary>
        /// Gets the radius of the ellipse.
        /// </summary>
        /// <returns>The radius.</returns>
        public Vector2 GetRadius();

        /// <summary>
        /// Gets the area of the ellipse.
        /// </summary>
        /// <returns>The area.</returns>
        public float GetArea()
        {
            var radius = GetRadius();
            return Mathf.PI * radius.x * radius.y;
        }

        /// <summary>
        /// Gets if a point is inside of the ellipse.
        /// </summary>
        /// <param name="transform">The transform of the ellipse.</param>
        /// <param name="point">The point to check.</param>
        /// <returns>Whether it is inside or not.</returns>
        public bool IsInside(SimpleTransform transform, Vector3 point);

        /// <summary>
        /// Gets the depenetration value required to get a point outside of the ellipse.
        /// </summary>
        /// <param name="transform">The transform of the ellipse.</param>
        /// <param name="point">The point to depenetrate.</param>
        /// <returns>The depenetration vector.</returns>
        public Vector3 GetDepenetration(SimpleTransform transform, Vector3 point);

        /// <summary>
        /// Gets the circumference of the ellipse.
        /// </summary>
        /// <returns>The circumference.</returns>
        public float GetCircumference()
        {
            var radius = GetRadius();

            // If this is a circle, we can just skip all the extra calcs
            if (radius.x == radius.y)
            {
                return 2f * Mathf.PI * radius.x;
            }
            // Otherwise, do an ellipse calc
            // This will return a semi-accurate approximation
            else
            {
                return 2f * Mathf.PI * Mathf.Sqrt((Mathf.Pow(radius.x, 2f) + Mathf.Pow(radius.y, 2f)) * 0.5f);
            }
        }
    }
}
