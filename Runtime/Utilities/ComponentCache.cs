using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared.Utilities {
    /// <summary>
    /// A utility for caching Unity objects to replace GetComponent.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ComponentCache<T> where T : Object {
        // The internal cache.
        private readonly Dictionary<GameObject, List<T>> _cache = new(new UnityComparer());

        /// <summary>
        /// Clears all cached components.
        /// </summary>
        public void Clear() => _cache.Clear();

        /// <summary>
        /// Returns the object from the given GameObject.
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public T Get(GameObject go) {
            if (_cache.ContainsKey(go)) 
                return _cache[go][0];
            else
                return null;
        }

        /// <summary>
        /// Returns true if the component was found and outputs the component.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public bool TryGet(GameObject go, out T comp) {
            if (_cache.ContainsKey(go)) {
                comp = _cache[go][0];
                return true;
            }
            else {
                comp = default;
                return false;
            }
        }

        /// <summary>
        /// Adds the component to the cache.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="comp"></param>
        public void Add(GameObject go, T comp) {
            if (_cache.ContainsKey(go) && !_cache[go].Contains(comp)) {
                _cache[go].Add(comp);
            }
            else {
                _cache.Add(go, new List<T>(1) { comp });
            }
        }

        /// <summary>
        /// Removes the GameObject from the cache.
        /// </summary>
        /// <param name="go"></param>
        public void Remove(GameObject go) => _cache.Remove(go);

        /// <summary>
        /// Removes the component from the cache.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="comp"></param>
        public void Remove(GameObject go, T comp) {
            if (_cache.ContainsKey(go))
                _cache[go].Remove(comp);
        }
    }

}