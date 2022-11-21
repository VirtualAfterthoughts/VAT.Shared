using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared.Utilities {
    /// <summary>
    /// A utility for caching Unity objects to replace GetComponent.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ComponentCache<T> where T : Object {
        // The internal dictionary of the cache.
        private readonly Dictionary<GameObject, T> cache = new(new UnityComparer());

        /// <summary>
        /// Clears all cached components.
        /// </summary>
        public void Clear() => cache.Clear();

        /// <summary>
        /// Returns the object from the given GameObject.
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public T Get(GameObject go) {
            if (cache.ContainsKey(go)) return cache[go];
            return null;
        }

        /// <summary>
        /// Returns true if the component was found and outputs the component.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public bool TryGet(GameObject go, out T comp) {
            comp = Get(go);
            return comp != null;
        }

        /// <summary>
        /// Adds the component to the cache.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="comp"></param>
        public void Add(GameObject go, T comp) {
            if (cache.ContainsKey(go)) cache.Remove(go);
            cache.Add(go, comp);
        }

        /// <summary>
        /// Tries to get the component on the GameObject. If not found, adds an existing component to the cache.
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public T AddOrGet(GameObject go) {
            T get = Get(go);
            if (get) return get;
            if (go.TryGetComponent(out T comp))
                Add(go, comp);
            return comp;
        }

        /// <summary>
        /// Removes the GameObject from the cache.
        /// </summary>
        /// <param name="go"></param>
        public void Remove(GameObject go) => cache.Remove(go);
    }

}