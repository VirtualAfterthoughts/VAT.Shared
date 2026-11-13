using System;

namespace VAT.Shared
{
    using UnityEngine;

    /// <summary>
    /// A reference to an interface implemented by a Unity object that can be serialized in the inspector.
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    [Serializable]
    public class InterfaceReference<TInterface> where TInterface : class
    {
        [SerializeField]
        private Object _target = null;

        /// <summary>
        /// The target Unity object that implements the interface.
        /// </summary>
        public Object Target => _target;

        /// <summary>
        /// The interface that is currently referenced.
        /// </summary>
        public TInterface Interface { get => _target as TInterface; set => _target = value as Object; }

        /// <summary>
        /// Constructs an interface reference with no value.
        /// </summary>
        public InterfaceReference() : this(null) { }

        /// <summary>
        /// Constructs an interface reference from an interface instance.
        /// </summary>
        /// <param name="target"></param>
        public InterfaceReference(TInterface target) { Interface = target; }
    }
}
