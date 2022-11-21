using UnityEngine;

namespace VAT.Shared.Extensions {
    public static class Modulation {
        public static float FloatMod(float mod, float min = 0.7f, float max = 1.3f) {
            return Mathf.LerpUnclamped(1f, Random.Range(min, max), mod);
        }
    }
}