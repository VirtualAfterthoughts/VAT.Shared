using System;

namespace VAT.Shared
{
    using UnityEngine;

    [Serializable]
    public class InterfaceReference<TInterface> where TInterface : class
    {
        [SerializeField]
        private Object _target = null;

        public InterfaceReference()
        {
            _target = null;
        }

        public InterfaceReference(TInterface target)
        {
            Interface = target;
        }

        public Object Target
        {
            get
            {
                return _target;
            }
        }

        public TInterface Interface
        {
            get
            {
                return _target as TInterface;
            }
            set
            {
                _target = value as Object;
            }
        }
    }
}
