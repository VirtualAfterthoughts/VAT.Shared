using UnityEngine;

namespace VAT.Shared.Extensions {
    /// <summary>
    /// Extension methods for floats.
    /// </summary>
    public static partial class FloatExtensions {
        /// <summary>
        /// Returns the average of each float.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static float Average(params float[] values) {
            return Sum(values) / values.Length;
        }

        /// <summary>
        /// Returns the sum of each float.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static float Sum(params float[] values) {
            float t = 0f;
            for (int i = 0; i < values.Length; i++)
                t += values[i];
            return t;
        }

        /// <summary>
        /// Returns the float rounded to the nearest int.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Rounded(this float f) => Mathf.Round(f);

        /// <summary>
        /// Returns this float rounded to the nearest place value.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        public static float Rounded(this float f, float place) => Rounded(f * place) / place;

        /// <summary>
        /// Returns the amount of digits in this number.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Length(this float f) => Mathf.Floor(Mathf.Log10(f) + 1);

        /// <summary>
        /// Divides lft by rht while checking for zero division.
        /// </summary>
        /// <param name="lft"></param>
        /// <param name="rht"></param>
        /// <returns></returns>
        public static float DivNoNan(this float lft, float rht) => rht != 0f ? lft / rht : 0f;

        /// <summary>
        /// Returns the distance between two floats.
        /// </summary>
        /// <param name="lft"></param>
        /// <param name="rht"></param>
        /// <returns></returns>
        public static float Distance(this float lft, float rht) => Mathf.Abs(lft - rht);

        /// <summary>
        /// Clamps float f between -1 and 1.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float SinClamp(this float f) => Mathf.Clamp(f, -1f, 1f);
    }
}
