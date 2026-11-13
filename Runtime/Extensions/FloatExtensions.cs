using UnityEngine;

namespace VAT.Shared.Extensions
{
    /// <summary>
    /// Extension functions for floats.
    /// </summary>
    public static class FloatExtensions
    {
        /// <summary>
        /// Clamps a float between -1 and 1.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ClampSigned(this float value) => Mathf.Clamp(value, -1f, 1f);

        /// <summary>
        /// Returns the average of an array of floats.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static float Average(params float[] values)
        {
            return Sum(values) / values.Length;
        }

        /// <summary>
        /// Returns the sum of an array of floats.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static float Sum(params float[] values)
        {
            float t = 0f;

            for (int i = 0; i < values.Length; i++)
            {
                t += values[i];
            }

            return t;
        }
    }
}
