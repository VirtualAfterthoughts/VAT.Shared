using UnityEngine;

namespace VAT.Shared.Utilities {
    /// <summary>
    /// Utilities for modulating values.
    /// </summary>
    public static class Modulation {
        /// <summary>
        /// Returns a mod multiplier for a float.
        /// </summary>
        /// <param name="mod">The amount to modulate.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The mod multiplier.</returns>
        public static float FloatMod(float mod, float min = 0.7f, float max = 1.3f) {
            return Mathf.LerpUnclamped(1f, Random.Range(min, max), mod);
        }
    }
}