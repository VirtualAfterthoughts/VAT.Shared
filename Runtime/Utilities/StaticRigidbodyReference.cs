using UnityEngine;

namespace VAT.Shared.Utilities
{
    public static class StaticRigidbodyReference
    {
        private static Rigidbody _instance = null;
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
    }
}
