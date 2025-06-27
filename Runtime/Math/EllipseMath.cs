using UnityEngine;

using VAT.Shared.Data;

namespace VAT.Shared.Math
{
    public static class EllipseMath
    {
        public static float GetArea(this Ellipse ellipse) => GetArea(ellipse.Radius);

        public static float GetArea(Vector2 radius) => GetArea(radius.x, radius.y);

        public static float GetArea(float a, float b) => Mathf.PI * a * b;

        public static float GetCircumference(this Ellipse ellipse) => GetCircumference(ellipse.Radius);

        public static float GetCircumference(Vector2 radius) => GetCircumference(radius.x, radius.y);

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

        public static float GetRoughCircumference(float a, float b) => Mathf.PI * (a + b);
    }
}
