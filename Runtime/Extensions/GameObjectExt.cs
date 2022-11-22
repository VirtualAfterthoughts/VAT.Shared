using UnityEngine;

namespace VAT.Shared.Extensions {
    /// <summary>
    /// Extension methods for GameObjects.
    /// </summary>
    public static partial class GameObjectExtensions {
        /// <summary>
        /// Attempts to get the component in parents.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="_this"></param>
        /// <param name="component">The component result.</param>
        /// <returns>The success.</returns>
        public static bool TryGetComponentInParent<T>(this GameObject _this, out T component) where T : Component
            => (component = _this.GetComponentInParent<T>()) != null;

        /// <summary>
        /// Attempts to get the component in parents.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="_this"></param>
        /// <param name="includeInactive">Should inactive components be included?</param>
        /// <param name="component">The component result.</param>
        /// <returns>The success.</returns>
        public static bool TryGetComponentInParent<T>(this GameObject _this, bool includeInactive, out T component) where T : Component
            => (component = _this.GetComponentInParent<T>(includeInactive)) != null;

        /// <summary>
        /// If the passed ref is null, it tries to get the component type in the parent. If it fails entirely, it returns false.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="_this"></param>
        /// <param name="component">The component ref.</param>
        /// <returns>The success.</returns>
        public static bool VerifyComponentInParent<T>(this GameObject _this, ref T component) where T : Component {
            bool found = component != null;
            if (!found)
                found = TryGetComponentInParent(_this, out component);
            return found;
        }


        /// <summary>
        /// If a component is found, it is returned. Otherwise, a new component is added.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="go">The GameObject.</param>
        /// <returns>The found or added component.</returns>
        public static T AddOrGetComponent<T>(this GameObject go) where T : Component {
            if (!go.TryGetComponent(out T comp))
                comp = go.AddComponent(typeof(T)) as T;
            return comp;
        }

        /// <summary>
        /// If the GameObject is missing a component of type T, it adds a new one.
        /// </summary>
        /// <typeparam name="T">The component typ.e</typeparam>
        /// <param name="go">The GameObject.</param>
        public static void AddMissingComponent<T>(this GameObject go) where T : Component {
            if (!go.TryGetComponent<T>(out _))
                go.AddComponent<T>();
        }

        /// <summary>
        /// Copies a component onto this GameObject.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="go">The GameObject to add the component to.</param>
        /// <param name="component">The component to copy.</param>
        /// <returns>The added component.</returns>
        public static T AddComponent<T>(this GameObject go, T component) where T : Component => go.AddComponent<T>().CopyComponent(component);
    }
}
