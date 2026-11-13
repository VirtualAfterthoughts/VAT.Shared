using UnityEngine;

namespace VAT.Shared.Utilities
{
    /// <summary>
    /// A static holder for a persistent kinematic rigidbody that exists at runtime.
    /// <para>
    /// This can be used to refresh joints when a connected body isn't available.
    /// </para>
    /// </summary>
    public static class StaticRigidbodyReference
    {
        /// <summary>
        /// The instance of the kinematic rigidbody. If it currently does not exist, one will be created.
        /// </summary>
        public static Rigidbody Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                var gameObject = new GameObject("Runtime Static Rigidbody")
                {
                    hideFlags = HideFlags.HideAndDontSave
                };

                _instance = gameObject.AddComponent<Rigidbody>();
                _instance.isKinematic = true;
                _instance.useGravity = false;

                return _instance;
            }
        }

        private static Rigidbody _instance = null;
    }
}
