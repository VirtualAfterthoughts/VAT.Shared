using UnityEngine;

namespace VAT.Shared.Utilities
{
    public static class ArrayRemapper
    {
        public static (T previous, T next, float blend) RemapElements<T>(int index, T[] original, T[] target)
        {
            var (remapIndex, remapBlend) = RemapIndex(index, original.Length, target.Length);

            var previous = original[remapIndex];
            var next = remapIndex + 1 < original.Length ? original[remapIndex + 1] : previous;

            return (previous, next, remapBlend);
        }

        public static (int index, float blend) RemapIndex(int index, int originalLength, int targetLength)
        {
            float percent = index / (float)targetLength;
            float originalValue = percent * originalLength;
            int originalIndex = Mathf.FloorToInt(originalValue);

            float blend = originalValue - originalIndex;

            return (originalIndex, blend);
        }
    }
}
