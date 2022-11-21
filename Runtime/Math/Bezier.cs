using UnityEngine;

namespace VAT.Shared.Math {
    /// <summary>
    /// Helper class for calculating bezier functions.
    /// </summary>
    public static class Bezier {
        public static float CalculateCubicBezier1D(float t, float p0, float t0, float p1, float t1)
        {
            float b0 = Mathf.Pow(1 - t, 3) * p0;
            float b1 = 3 * Mathf.Pow(1 - t, 2) * t * t0;
            float b2 = 3 * (1 - t) * Mathf.Pow(t, 2) * t1;
            float b3 = Mathf.Pow(t, 3) * p1;

            return b0 + b1 + b2 + b3;
        }

        public static Vector3 CalculateCubicBezier3D(float t, Vector3 p0, Vector3 t0, Vector3 p1, Vector3 t1) {
            t = Mathf.Clamp01(t);

            float x = CalculateCubicBezier1D(t, p0.x, t0.x, p1.x, t1.x);
            float y = CalculateCubicBezier1D(t, p0.y, t0.y, p1.y, t1.y);
            float z = CalculateCubicBezier1D(t, p0.z, t0.z, p1.z, t1.z);

            return new Vector3(x, y, z);
        }
    }
}