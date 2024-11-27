using System;
using System.Linq;

namespace VAT.Shared.Editor
{
    using UnityEngine;
    using UnityEngine.UIElements;

    using UnityEditor;
    using UnityEditor.UIElements;

    [CustomPropertyDrawer(typeof(InterfaceReference<>))]
    public class InterfaceReferenceEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var targetProperty = property.FindPropertyRelative("_target");

            // Get our types
            var interfaceType = property.GetPropertyInstance().GetType().GenericTypeArguments[0];
            var dragType = typeof(Object);

            // Create the object field for the interface reference
            var objectField = new ObjectField(property.displayName);
            objectField.AddToClassList(ObjectField.alignedFieldUssClassName);
            objectField.BindProperty(targetProperty);
            objectField.objectType = interfaceType;

            // Only allow drag and drop for the interface type
            objectField.RegisterCallback<DragUpdatedEvent>(dragUpdated =>
            {
                if (!IsDraggingType(interfaceType))
                {
                    dragUpdated.PreventDefault();
                    objectField.objectType = interfaceType;
                }
                else
                {
                    objectField.objectType = dragType;
                }
            });

            // Reset to the interface type on drag exit
            objectField.RegisterCallback<DragExitedEvent>(dragExited =>
            {
                objectField.objectType = interfaceType;
            });

            // Make sure to validate the value
            objectField.RegisterValueChangedCallback(changed =>
            {
                var newValue = changed.newValue;

                if (newValue is GameObject go)
                {
                    objectField.value = go.GetComponent(interfaceType);
                }
            });

            return objectField;
        }

        private static bool IsDraggingType(Type type)
        {
            return DragAndDrop.objectReferences.Any(obj => type.IsAssignableFrom(obj.GetType()) || (obj is GameObject go && go.GetComponent(type)) );
        }
    }
}
