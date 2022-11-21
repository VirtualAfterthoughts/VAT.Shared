using UnityEngine;

namespace VAT.Shared.Math {
    /// <summary>
    /// Helper class for converting between metric and imperial units.
    /// </summary>
    public static partial class Measurements {
        /// <summary>
        /// Formats the meters into a string.
        /// </summary>
        /// <param name="meters">The meters.</param>
        /// <returns>The metric string.</returns>
        public static string FormatMetric(float meters) => $"{meters}m";

        /// <summary>
        /// Formats the meters into a string in feet and inches.
        /// </summary>
        /// <param name="meters">The meters.</param>
        /// <returns>The imperial string.</returns>
        public static string FormatImperial(float meters) {
            float sign = Mathf.Sign(meters);

            meters = Mathf.Abs(meters);

            float total = ConvertToFeet(meters);

            float feet = Mathf.Floor(total);
            float inches = Mathf.Floor((total - feet) * 12f);

            return $"{feet * sign}' {inches * sign}\" ";
        }

        /// <summary>
        /// Converts the meters into feet.
        /// </summary>
        /// <param name="meters">The meters.</param>
        /// <returns>The feet.</returns>
        public static float ConvertToFeet(float meters) {
            return meters * 3.281f;
        }
    }
}