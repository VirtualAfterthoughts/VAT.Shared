using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared.Utilities
{
    /// <summary>
    /// A global cache for components attached to GameObjects to avoid the costliness of GetComponent.
    /// <para>
    /// The addition and removal of components needs to occur manually to avoid accidental persistent references of the components.
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ComponentCache<T>
    {
        /// <summary>
        /// The internal cache.
        /// </summary>
        private readonly Dictionary<int, List<T>> _cache = new();

        /// <summary>
        /// Clears the component cache.
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
        }

        /// <summary>
        /// Gets the first component at a GameObject instance ID, or null if none is found.
        /// </summary>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        public T Get(int instanceID)
        {
            TryGet(instanceID, out var component);
            return component;
        }

        /// <summary>
        /// Gets the first component on a GameObject, or null if none is found.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public T Get(GameObject gameObject) => Get(gameObject.GetInstanceID());

        /// <summary>
        /// Gets the list of all components at a GameObject instance ID, or null if the GameObject has not been added to the cache.
        /// </summary>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        public List<T> GetAll(int instanceID)
        {
            if (_cache.TryGetValue(instanceID, out var list))
            {
                return list;
            }

            return null;
        }

        /// <summary>
        /// Gets the list of all components on a GameObject, or null if the GameObject has not been added to the cache.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public List<T> GetAll(GameObject gameObject) => GetAll(gameObject.GetInstanceID());

        /// <summary>
        /// Tries to get the first component at a GameObject instance ID and returns if it succeeded.
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool TryGet(int instanceID, out T component)
        {
            if (_cache.TryGetValue(instanceID, out var list) && list.Count > 0)
            {
                component = list[0];
                return true;
            }

            component = default;
            return false;
        }

        /// <summary>
        /// Tries to get the first component on a GameObject and returns if it succeeded.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool TryGet(GameObject gameObject, out T component) => TryGet(gameObject.GetInstanceID(), out component);

        /// <summary>
        /// Returns true if the GameObject instance ID has been added to the cache.
        /// </summary>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        public bool Contains(int instanceID) => _cache.ContainsKey(instanceID);

        /// <summary>
        /// Returns true if the GameObject has been added to the cache.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool Contains(GameObject gameObject) => Contains(gameObject.GetInstanceID());

        /// <summary>
        /// Adds a component to the cache with a given GameObject instance ID.
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="component"></param>
        public void Add(int instanceID, T component)
        {
            if (Contains(instanceID) && !_cache[instanceID].Contains(component))
            {
                _cache[instanceID].Add(component);
            }
            else
            {
                _cache.Add(instanceID, new List<T>(1) { component });
            }
        }

        /// <summary>
        /// Adds a component to the cache with a given GameObject.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="component"></param>
        public void Add(GameObject gameObject, T component) => Add(gameObject.GetInstanceID(), component);

        /// <summary>
        /// Removes a GameObject instance ID from the cache.
        /// </summary>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        public bool Remove(int instanceID) => _cache.Remove(instanceID);

        /// <summary>
        /// Removes a GameObject from the cache.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool Remove(GameObject gameObject) => Remove(gameObject.GetInstanceID());

        /// <summary>
        /// Removes a given component on a GameObject instance ID from the cache.
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool Remove(int instanceID, T component)
        {
            if (!_cache.TryGetValue(instanceID, out var list))
            {
                return false;
            }

            bool removed = list.Remove(component);

            if (list.Count <= 0)
            {
                _cache.Remove(instanceID);
            }

            return removed;
        }

        /// <summary>
        /// Removes a given component on a GameObject from the cache.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool Remove(GameObject gameObject, T component) => Remove(gameObject.GetInstanceID(), component);
    }

}