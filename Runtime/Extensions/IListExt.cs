using System.Collections.Generic;

namespace VAT.Shared.Extensions {
    /// <summary>
    /// Extension methods for ILists.
    /// </summary>
    public static partial class IListExtensions {
        /// <summary>
        /// Returns the first element in the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T First<T>(this IList<T> list) => list[0];

        /// <summary>
        /// Returns the first element in the list, or default if it is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T FirstOrDefault<T>(this IList<T> list) {
            if (list.Count <= 0) return default;
            return list[0];
        }

        /// <summary>
        /// Returns the last element in the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T Last<T>(this IList<T> list) => list[list.Count - 1];

        /// <summary>
        /// Returns the last element in the list, or default if it is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T LastOrDefault<T>(this IList<T> list) {
            if (list.Count <= 0) return default;
            return list[list.Count - 1];
        }

        /// <summary>
        /// Appends an item to the end of the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        public static void Append<T>(this IList<T> list, T item) {
            list.Remove(item);
            list.Add(item);
        }

        /// <summary>
        /// Prepends the item to the beginning of the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        public static void Prepend<T>(this IList<T> list, T item) {
            list.Remove(item);
            list.Insert(0, item);
        }

        /// <summary>
        /// Tries to add the element to the list. If it already exists, false is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool TryAdd<T>(this IList<T> list, T item) {
            if (list.Contains(item)) return false;
            list.Add(item); return true;
        }

        /// <summary>
        /// Tries to add a key and value to the dictionary. If the key already exists, false is returned.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value) {
            if (dict.ContainsKey(key)) return false;
            dict.Add(key, value); return true;
        }
    }
}