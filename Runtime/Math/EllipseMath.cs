using UnityEngine;

using VAT.Shared.Data;

namespace VAT.Shared.Math
{
    /// <summary>
    /// Functions for calculating values of ellipses.
    /// </summary>
    public static class EllipseMath
    {
        /// <summary>
        /// Gets the area of an ellipse.
        /// </summary>
        /// <param name="ellipse"></param>
        /// <returns></returns>
        public static float GetArea(this Ellipse ellipse) => GetArea(ellipse.Radius);

        /// <summary>
        /// Gets the area of an ellipse with a given radius.
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static float GetArea(Vector2 radius) => GetArea(radius.x, radius.y);

        /// <summary>
        /// Gets the area of an ellipse with the given a and b dimensions.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float GetArea(float a, float b) => Mathf.PI * a * b;

        /// <summary>
        /// Gets the circumference of an ellipse.
        /// </summary>
        /// <param name="ellipse"></param>
        /// <returns></returns>
        public static float GetCircumference(this Ellipse ellipse) => GetCircumference(ellipse.Radius);
        
        /// <summary>
        /// Gets the circumference of an ellipse with a given radius.
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static float GetCircumference(Vector2 radius) => GetCircumference(radius.x, radius.y);

        /// <summary>
        /// Gets the circumference of an ellipse with the given a and b dimensions.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float GetCircumference(float a, float b)
        {
            float aSubB = a - b;
            float aAddB = a + b;

            float h = (aSubB * aSubB) / (aAddB * aAddB);
            float h3 = 3f * h;

            float rough = GetRoughCircumference(a, b);

            float frac = h3 / (10f + Mathf.Sqrt(4f - h3));

            float circumference = rough * (1f + frac);

            return circumference;
        }

        /// <summary>
        /// Gets a quick and rough approximation of the circumference of an ellipse.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float GetRoughCircumference(float a, float b) => Mathf.PI * (a + b);
    }
}
