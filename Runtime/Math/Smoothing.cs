using UnityEngine;

namespace VAT.Shared.Math
{
    /// <summary>
    /// Functions for smoothing values between frames.
    /// </summary>
    public static class Smoothing
    {
        /// <summary>
        /// Calculates a t value for smoothing a lerp consistently between frames.
        /// <para>This is less performant than <see cref="CalculateDecay(float, float)"/>.</para>
        /// </summary>
        /// <param name="smoothing"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static float CalculateSmoothing(float smoothing, float deltaTime)
        {
            return 1f - Mathf.Pow(smoothing, deltaTime);
        }

        /// <summary>
        /// Calculates a t value for smoothing a lerp consistently between frames.
        /// </summary>
        /// <param name="decay"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static float CalculateDecay(float decay, float deltaTime)
        {
            return 1f - Mathf.Exp(-decay * deltaTime);
        }
    }
}
