using System.Collections.Generic;

namespace VAT.Shared.Extensions
{
    /// <summary>
    /// Extension functions for lists.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Tries to add an item to a list if it is not already present.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool TryAdd<T>(this List<T> list, T item)
        {
            if (list.Contains(item))
            {
                return false;
            }

            list.Add(item);
            return true;
        }

        /// <summary>
        /// Gets an element in a list by an index, or adds a new one using a default constructor if none is present.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetOrAdd<T>(this List<T> list, int index) where T : new()
        {
            if (index < list.Count)
            {
                return list[index];
            }

            var item = new T();
            list.Add(item);

            return item;
        }

        /// <summary>
        /// Removes elements in a list past a certain count.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="maxCount"></param>
        public static void RemoveExcess<T>(this List<T> list, int maxCount)
        {
            for (var i = 0; i < list.Count - maxCount; i++)
            {
                list.RemoveAt(list.Count - 1);
            }
        }
    }
}
