using UnityEngine;

using System;
using System.Reflection;

namespace VAT.Shared.Extensions {
    public static partial class ReflectionExtensions {
        // Extra binding flags for reflection
        public static readonly BindingFlags AllFields = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        // Gets types from string
        public static Type TypeOf(this string str) => Type.GetType(str);
        public static Type TypeOf(this object obj) => Type.GetType(obj.ToString());

        // Copies other onto new component comp
        public static T CopyComponent<T>(this Component comp, T other) where T : Component {
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
            return comp as T;
        }

        public static T AddComponent<T>(this GameObject go, T toCopy) where T : Component => go.AddComponent<T>().CopyComponent(toCopy);
    }
}