using System.Reflection;
using System;
using UnityEngine;

namespace VAT.Shared.Extensions {
    /// <summary>
    /// Extension methods for components.
    /// </summary>
    public static class ComponentExtensions {
        /// <summary>
        /// Attempts to get the component in parents.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="_this"></param>
        /// <param name="component">The component result.</param>
        /// <returns>The success.</returns>
        public static bool TryGetComponentInParent<T>(this Component _this, out T component) where T : Component
            => _this.gameObject.TryGetComponentInParent(out component);

        /// <summary>
        /// Attempts to get the component in parents.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="_this"></param>
        /// <param name="includeInactive">Should inactive components be included?</param>
        /// <param name="component">The component result.</param>
        /// <returns>The success.</returns>
        public static bool TryGetComponentInParent<T>(this Component _this, bool includeInactive, out T component) where T : Component
            => _this.gameObject.TryGetComponentInParent(includeInactive, out component);

        /// <summary>
        /// If the passed ref is null, it tries to get the component type in the parent. If it fails entirely, it returns false.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="_this"></param>
        /// <param name="component">The component ref.</param>
        /// <returns>The success.</returns>
        public static bool VerifyComponentInParent<T>(this Component _this, ref T component) where T : Component
            => _this.gameObject.VerifyComponentInParent(ref component);

        /// <summary>
        /// Copies another component's values onto this component.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="comp">This component to copy to.</param>
        /// <param name="other">The component to copy from.</param>
        /// <returns>This component.</returns>
        public static T CopyComponent<T>(this T comp, T other) where T : Component {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match

            // Get all the properties from the type
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] props = type.GetProperties(flags);

            // Loop through the properties and set the values
            for (int i = 0; i < props.Length; i++) {
                PropertyInfo info = props[i];
                if (info.CanWrite) {
                    try {
                        info.SetValue(comp, info.GetValue(other, null), null);
                    }
                    catch (Exception e) {
                        Debug.LogWarning($"Failed setting property with reason: {e.Message}\nTrace:{e.StackTrace}");
                    }
                }
            }

            // Get all the fields from the type
            FieldInfo[] fields = type.GetFields(flags);

            // Loop through the fields and set the values
            for (int i = 0; i < fields.Length; i++)
                fields[i].SetValue(comp, fields[i].GetValue(other));
            return comp;
        }

        /// <summary>
        /// Checks if this component is tagged as a player.
        /// </summary>
        /// <param name="_this"></param>
        /// <returns>If the tag is Player.</returns>
        public static bool IsPlayer(this Component _this) => _this.CompareTag("Player");
    }
}
