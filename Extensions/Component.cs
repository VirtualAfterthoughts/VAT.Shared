using UnityEngine;

namespace VAT.Shared.Extensions {
    public static class ComponentExtensions {
        // Same as TryGetComponent but for GetComponentInParent
        public static bool TryGetComponentInParent<T>(this MonoBehaviour _this, out T component) where T : Component => (component = _this.GetComponentInParent<T>()) != null;

        // If the passed ref is null, it tries to get said component in the parent, if it fails entirely, it returns false
        public static bool VerifyComponentInParent<T>(this MonoBehaviour _this, ref T component) where T : Component {
            bool found = component != null;
            if (!found)
                found = TryGetComponentInParent(_this, out component);
            return found;
        }

        // Is this component tagged as a player?
        public static bool IsPlayer(this Component _this) => _this.CompareTag("Player");
    }
}
