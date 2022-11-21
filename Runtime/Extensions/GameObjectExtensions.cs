using UnityEngine;

namespace VAT.Shared.Extensions {
    public static partial class GameObjectExtensions {
        // Same as TryGetComponent but for GetComponentInParent
        public static bool TryGetComponentInParent<T>(this GameObject _this, out T component) where T : Component => (component = _this.GetComponentInParent<T>()) != null;

        // If the passed ref is null, it tries to get said component in the parent, if it fails entirely, it returns false
        public static bool VerifyComponentInParent<T>(this GameObject _this, ref T component) where T : Component {
            bool found = component != null;
            if (!found)
                found = TryGetComponentInParent(_this, out component);
            return found;
        }

        // Adds a component to the GameObject or returns existing if found
        public static T AddOrGetComponent<T>(this GameObject go) where T : Component {
            if (!go.TryGetComponent(out T comp))
                comp = go.AddComponent(typeof(T)) as T;
            return comp;
        }

        /// <summary>
        /// If the GameObject is missing a component of type T, it adds a new one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        public static void AddMissingComponent<T>(this GameObject go) where T : Component {
            if (!go.TryGetComponent<T>(out _))
                go.AddComponent<T>();
        }
    }
}
