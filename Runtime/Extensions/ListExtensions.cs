using System.Collections.Generic;

namespace VAT.Shared.Extensions
{
    public static class ListExtensions
    {
        public static bool TryAdd<T>(this List<T> list, T item)
        {
            if (list.Contains(item))
            {
                return false;
            }

            list.Add(item);
            return true;
        }

        public static T GetOrAdd<T>(List<T> list, int index) where T : new()
        {
            if (index < list.Count)
            {
                return list[index];
            }

            var item = new T();
            list.Add(item);

            return item;
        }

        public static void RemoveExcess<T>(ref List<T> list, int maxCount)
        {
            for (var i = 0; i < list.Count - maxCount; i++)
            {
                list.RemoveAt(list.Count - 1);
            }
        }
    }
}
