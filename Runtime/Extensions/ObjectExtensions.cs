using UnityEngine;

namespace VAT.Shared.Extensions
{
    /// <summary>
    /// Extension functions for Unity Objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Removes a GameObject, component, or asset if it exists, or returns false otherwise.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool TryDestroy(this Object obj)
        {
            return TryDestroy(obj, 0f);
        }

        /// <summary>
        /// Removes a GameObject, component, or asset if it exists, or returns false otherwise.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool TryDestroy(this Object obj, float t)
        {
            if (obj != null)
            {
                Object.Destroy(obj, t);
                return true;
            }

            return false;
        }
    }
}
