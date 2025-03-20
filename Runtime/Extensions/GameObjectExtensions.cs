using UnityEngine;

namespace VAT.Shared.Extensions
{
    public static class GameObjectExtensions
    {
        public static bool TryGetComponentInParent<T>(this GameObject go, out T component) where T : Component
        {
            component = go.GetComponentInParent<T>();

            return component != null;
        }    

        public static T AddOrGetComponent<T>(this GameObject go) where T : Component
        {
            if (!go.TryGetComponent(out T comp))
            {
                comp = go.AddComponent<T>();
            }

            return comp;
        }
    }
}
