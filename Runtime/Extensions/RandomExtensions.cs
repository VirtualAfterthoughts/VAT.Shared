using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared.Extensions
{
    public static class RandomExtensions
    {
        public static T GetRandom<T>(this IList<T> list)
        {
            if (list.Count <= 0)
            {
                return default;
            }

            return list[Random.Range(0, list.Count)];
        }
    }
}