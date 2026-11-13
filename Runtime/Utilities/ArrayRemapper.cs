using System;

using UnityEngine;

namespace VAT.Shared.Utilities
{
    /// <summary>
    /// Functions for remapping arrays of different sizes to new sizes while blending the elements.
    /// </summary>
    public static class ArrayRemapper
    {
        /// <summary>
        /// Returns a new element for an array at a specified index by blending between elements of another array of a different size.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="original"></param>
        /// <param name="targetLength"></param>
        /// <param name="remapFunction"></param>
        /// <returns></returns>
        public static T RemapElement<T>(int index, T[] original, int targetLength, Func<T, T, float, T> remapFunction)
        {
            var (previous, next, blend) = RemapElement(index, original, targetLength);
            return remapFunction(previous, next, blend);
        }

        /// <summary>
        /// Returns two elements from an array and the blend amount needed to create an element in a new array of a different length at a specified index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="original"></param>
        /// <param name="targetLength"></param>
        /// <returns></returns>
        public static (T previous, T next, float blend) RemapElement<T>(int index, T[] original, int targetLength)
        {
            var (remapIndex, remapBlend) = RemapIndex(index, original.Length, targetLength);

            var previous = original[remapIndex];
            var next = remapIndex + 1 < original.Length ? original[remapIndex + 1] : previous;

            return (previous, next, remapBlend);
        }

        /// <summary>
        /// Returns a remapped array index and blend value given the original array length and the target array length.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="originalLength"></param>
        /// <param name="targetLength"></param>
        /// <returns></returns>
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
