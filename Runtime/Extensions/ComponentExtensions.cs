using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared.Extensions
{
    public static class ComponentExtensions
    {
        /// <summary>
        /// Returns every <typeparamref name="TComponent"/> in the parent's children that are not under another instance of <typeparamref name="TParent"/>.
        /// <para>It is recommended to avoid using this at runtime or on Update if possible as it invokes many GetComponents and allocates new lists.</para>
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static TComponent[] GetAttachedComponents<TParent, TComponent>(this TParent parent) where TParent : Component where TComponent : Component
        {
            var childComponents = parent.GetComponentsInChildren<TComponent>();

            List<TComponent> validComponents = new();

            foreach (var component in childComponents)
            {
                if (component.GetComponentInParent<TParent>(true) != parent)
                {
                    continue;
                }

                validComponents.Add(component);
            }

            return validComponents.ToArray();
        }
    }
}
