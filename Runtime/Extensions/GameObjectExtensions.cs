using UnityEngine;

namespace VAT.Shared.Extensions
{
    /// <summary>
    /// Extension functions for GameObjects.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Tries to get a component in the parent of a GameObject and returns if one was found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static bool TryGetComponentInParent<T>(this GameObject go, out T component) where T : Component
        {
            component = go.GetComponentInParent<T>();

            return component != null;
        }    

        /// <summary>
        /// Gets a component on a GameObject, or adds one if it is missing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if (!go.TryGetComponent(out T comp))
            {
                comp = go.AddComponent<T>();
            }

            return comp;
        }
    }
}
