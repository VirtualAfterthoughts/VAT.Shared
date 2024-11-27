using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared.Utilities
{
    public class ComponentCache<T>
    {
        private readonly Dictionary<GameObject, List<T>> _cache = new(new UnityComparer());

        public void Clear()
        {
            _cache.Clear();
        }

        public T Get(GameObject go)
        {
            if (Contains(go))
            {
                return _cache[go][0];
            }
            else
            {
                return default;
            }
        }

        public List<T> GetAll(GameObject go)
        {
            if (!Contains(go))
            {
                return _cache[go] = new();
            }

            return _cache[go];
        }

        public bool TryGet(GameObject go, out T component)
        {
            if (Contains(go))
            {
                component = _cache[go][0];
                return true;
            }

            component = default;
            return false;
        }

        public bool Contains(GameObject go)
        {
            return _cache.ContainsKey(go);
        }

        public void Add(GameObject go, T component)
        {
            if (Contains(go) && !_cache[go].Contains(component))
            {
                _cache[go].Add(component);
            }
            else
            {
                _cache.Add(go, new List<T>(1) { component });
            }
        }

        public bool Remove(GameObject go)
        {
            return _cache.Remove(go);
        }

        public bool Remove(GameObject go, T component)
        {
            if (Contains(go))
            {
                return _cache[go].Remove(component);
            }

            return false;
        }
    }

}