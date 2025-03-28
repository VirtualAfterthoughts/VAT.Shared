using UnityEngine;

namespace VAT.Shared.Math
{
    public static class Smoothing
    {
        public static float CalculateSmoothing(float smoothing, float deltaTime)
        {
            return 1f - Mathf.Pow(smoothing, deltaTime);
        }

        public static float CalculateDecay(float decay, float deltaTime)
        {
            return 1f - Mathf.Exp(-decay * deltaTime);
        }
    }
}
