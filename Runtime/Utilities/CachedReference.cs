using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared
{
    /// <summary>
    /// A reference to a behaviour with cached gameObject and transform variables.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class CachedReference<T> where T : Behaviour {
        public T component;

        private GameObject _gameObject = null;
        public GameObject gameObject { 
            get 
            {
                if (_gameObject == null && component != null)
                    _gameObject = component.gameObject;

                return _gameObject;
            } 
        }

        private Transform _transform = null;
        public Transform transform { 
            get {
                if (_transform == null && component != null)
                    _transform = component.transform;

                return _transform;
            } 
        }
    }
}
