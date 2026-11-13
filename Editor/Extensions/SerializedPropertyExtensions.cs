using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

using UnityEditor;

namespace VAT.Shared.Editor
{
    /// <summary>
    /// Extension functions for SerializedProperty instances.
    /// </summary>
    public static class SerializedPropertyExtensions
    {
        // Original: https://gist.github.com/aholkner/214628a05b15f0bb169660945ac7923b
        /// <summary>
        /// Returns the object instance that this SerializedProperty is derived from.
        /// <br></br>
        /// Only use for complex editors when necessary.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object GetPropertyInstance(this SerializedProperty property)
        {
            string propertyPath = property.propertyPath;
            object value = property.serializedObject.targetObject;
            int i = 0;
            while (NextPathComponent(propertyPath, ref i, out var token))
                value = GetPathComponentValue(value, token);
            return value;
        }

        private struct PropertyPathComponent
        {
            public string PropertyName;

            public int ElementIndex;
        }

        private static readonly Regex _arrayElementRegex = new(@"\GArray\.data\[(\d+)\]", RegexOptions.Compiled);

        private static bool NextPathComponent(string propertyPath, ref int index, out PropertyPathComponent component)
        {
            component = new PropertyPathComponent();

            if (index >= propertyPath.Length)
                return false;

            var arrayElementMatch = _arrayElementRegex.Match(propertyPath, index);
            if (arrayElementMatch.Success)
            {
                index += arrayElementMatch.Length + 1; // Skip past next '.'
                component.ElementIndex = int.Parse(arrayElementMatch.Groups[1].Value);
                return true;
            }

            int dot = propertyPath.IndexOf('.', index);
            if (dot == -1)
            {
                component.PropertyName = propertyPath.Substring(index);
                index = propertyPath.Length;
            }
            else
            {
                component.PropertyName = propertyPath.Substring(index, dot - index);
                index = dot + 1; // Skip past next '.'
            }

            return true;
        }

        private static object GetPathComponentValue(object container, PropertyPathComponent component)
        {
            if (component.PropertyName == null)
                return ((IList)container)[component.ElementIndex];
            else
                return GetMemberValue(container, component.PropertyName);
        }

        private static object GetMemberValue(object container, string name)
        {
            if (container == null)
                return null;
            var type = container.GetType();
            while (type.BaseType != null)
            {
                var members = type.GetMember(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                for (int i = 0; i < members.Length; ++i)
                {
                    if (members[i] is FieldInfo field)
                        return field.GetValue(container);
                    else if (members[i] is PropertyInfo property)
                        return property.GetValue(container);
                }

                type = type.BaseType;
            }
            return null;
        }

    }
}
