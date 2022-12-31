using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared {
    /// <summary>
    /// An interface to implement for caching the behaviour's GameObject and Transform variables.
    /// </summary>
    public interface ICachedMonoBehaviour {
        GameObject GameObject { get; }
        Transform Transform { get; }
    }

    /// <summary>
    /// An implementation of <see cref="ICachedMonoBehaviour"/>.
    /// </summary>
    public class CachedMonoBehaviour : MonoBehaviour, ICachedMonoBehaviour {
        private GameObject _gameObject = null;
        public GameObject GameObject {
            get  {
                if (_gameObject == null)
                    _gameObject = base.gameObject;

                return _gameObject;
            } 
        }

        private Transform _transform = null;
        public Transform Transform {
            get {
                if (_transform == null)
                    _transform = base.transform;

                return _transform;
            }
        }
    }
}
