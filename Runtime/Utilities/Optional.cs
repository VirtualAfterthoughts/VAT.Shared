using System;

using UnityEngine;

namespace VAT.Shared.Utilities {
    /// <summary>
    /// Structure for enabling and disabling variables.
    /// <para> Source: https://gist.github.com/aarthificial/f2dbb58e4dbafd0a93713a380b9612af </para>
    /// </summary>
    /// <typeparam name="T">The variable type.</typeparam>
    [Serializable]
    public struct Optional<T> {
        [SerializeField] private bool enabled;
        [SerializeField] private T value;

        public bool Enabled => enabled;
        public T Value => value;

        public Optional(T value) {
            enabled = true;
            this.value = value;
        }
    }
}
