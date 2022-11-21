using UnityEngine;

namespace VAT.Shared.Extensions {
    public static partial class FloatExtensions {
        // Returns the average of all values
        public static float Average(params float[] values) {
            return Total(values) / values.Length;
        }

        // Returns the total of all values
        public static float Total(params float[] values) {
            float t = 0f;
            for (int i = 0; i < values.Length; i++)
                t += values[i];
            return t;
        }

        // Returns this float rounded to the nearest int.
        public static float Rounded(this float f) => Mathf.Round(f);

        // Returns this float rounded to the nearest place value (ex. 10ths place = 10)
        public static float Rounded(this float f, float p) => Rounded(f * p) / p;

        // Returns the amount of digits in the number.
        public static float Length(this float f) => Mathf.Floor(Mathf.Log10(f) + 1);

        // Divides lft by rht without throwing NaNs incase of 0 division
        public static float DivNoNan(this float lft, float rht) => rht != 0f ? lft / rht : 0f;

        public static float Distance(this float lft, float rht) => Mathf.Abs(lft - rht);

        /// <summary>
        /// Clamps float f between -1 and 1.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float SinClamp(this float f) => Mathf.Clamp(f, -1f, 1f);
    }
}
