using UnityEngine;

namespace VAT.Shared.Extensions
{
    public static class FloatExtensions
    {
        /// <summary>
        /// Clamps value between -1 and 1 and returns value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ClampSine(this float value) => Mathf.Clamp(value, -1f, 1f);

        public static float Average(params float[] values)
        {
            return Sum(values) / values.Length;
        }

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
