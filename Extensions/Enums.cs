using System;

namespace VAT.Shared.Extensions {
    public static partial class EnumExtensions {
        public static T Next<T>(this T e) => (T)e.Next(typeof(T));

        public static object Next(this object e, Type T){
            if (!T.IsEnum) throw new ArgumentException($"Argument {T.FullName} is not an Enum.");

            Array Arr = Enum.GetValues(T);
            int idx = Array.IndexOf(Arr, e) + 1;
            return (Arr.Length == idx) ? Arr.GetValue(0) : Arr.GetValue(idx);
        }

        public static T Prev<T>(this T e) => (T)e.Prev(typeof(T));

        public static object Prev(this object e, Type T) {
            if (!T.IsEnum) throw new ArgumentException($"Argument {T.FullName} is not an Enum.");

            Array Arr = Enum.GetValues(T);
            int idx = Array.IndexOf(Arr, e) - 1;
            int bound = Arr.GetLowerBound(0);
            return (idx < bound) ? Arr.GetValue(Arr.Length - 1) : Arr.GetValue(idx);
        }
    }
}