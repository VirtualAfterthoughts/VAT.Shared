using System.Collections.Generic;

namespace VAT.Shared.Extensions {
    public static partial class IListExtensions {
        public static T First<T>(this IList<T> list) => list[0];

        public static T FirstOrDefault<T>(this IList<T> list) {
            if (list.Count <= 0) return default;
            return list[0];
        }

        public static T Last<T>(this IList<T> list) => list[list.Count - 1];

        public static T LastOrDefault<T>(this IList<T> list) {
            if (list.Count <= 0) return default;
            return list[list.Count - 1];
        }

        public static void Append<T>(this IList<T> list, T item) {
            list.Remove(item);
            list.Add(item);
        }

        public static void Prepend<T>(this IList<T> list, T item) {
            list.Remove(item);
            list.Insert(0, item);
        }

        public static bool TryAdd<T>(this IList<T> list, T item) {
            if (list.Contains(item)) return false;
            list.Add(item); return true;
        }

        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value) {
            if (dict.ContainsKey(key)) return false;
            dict.Add(key, value); return true;
        }
    }
}