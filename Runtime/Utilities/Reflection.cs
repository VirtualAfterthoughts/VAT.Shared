using UnityEngine;

using System;
using System.Reflection;

namespace VAT.Shared.Utilities {
    /// <summary>
    /// Utilities for reflection.
    /// </summary>
    public static partial class Reflection {
        /// <summary>
        /// BindingFlags that contain all fields.
        /// </summary>
        public static readonly BindingFlags AllFields = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// Returns a type using a string.
        /// </summary>
        /// <param name="value">The type string.</param>
        /// <returns>The type.</returns>
        public static Type TypeOf(this string value) => Type.GetType(value);

        /// <summary>
        /// Returns the type of the object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The type.</returns>
        public static Type TypeOf(this object obj) => Type.GetType(obj.ToString());
    }
}