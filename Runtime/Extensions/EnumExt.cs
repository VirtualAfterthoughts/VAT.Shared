using System;

namespace VAT.Shared.Extensions {
    /// <summary>
    /// Extension methods for enums.
    /// </summary>
    public static partial class EnumExtensions {
        /// <summary>
        /// Returns the next value after this enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static T Next<T>(this T e) => (T)e.Next(typeof(T));

        /// <summary>
        /// Returns the next value after this enum.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static object Next(this object e, Type T){
            if (!T.IsEnum) 
                throw new ArgumentException($"Argument {T.FullName} is not an Enum.");

            Array Arr = Enum.GetValues(T);
            int idx = Array.IndexOf(Arr, e) + 1;
            return (Arr.Length == idx) ? Arr.GetValue(0) : Arr.GetValue(idx);
        }

        /// <summary>
        /// Returns the previous value before this enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static T Prev<T>(this T e) => (T)e.Prev(typeof(T));

        /// <summary>
        /// Returns the previous value before this enum.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static object Prev(this object e, Type T) {
            if (!T.IsEnum) throw new ArgumentException($"Argument {T.FullName} is not an Enum.");

            Array Arr = Enum.GetValues(T);
            int idx = Array.IndexOf(Arr, e) - 1;
            int bound = Arr.GetLowerBound(0);
            return (idx < bound) ? Arr.GetValue(Arr.Length - 1) : Arr.GetValue(idx);
        }
    }
}