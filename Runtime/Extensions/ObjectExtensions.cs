using UnityEngine;

namespace VAT.Shared.Extensions
{
    public static class ObjectExtensions
    {
        public static bool TryDestroy(this Object obj)
        {
            return TryDestroy(obj, 0f);
        }

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
