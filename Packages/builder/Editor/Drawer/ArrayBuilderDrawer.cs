using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Blob.Editor
{
    [CustomPropertyDrawer(typeof(ArrayBuilder<>), useForChildren: true)]
    public class ArrayBuilderDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var valueProperty = ValueProperty(property);
            return EditorGUI.GetPropertyHeight(valueProperty, GUIContent.none, includeChildren: true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();
            var valueProperty = ValueProperty(property);
            var elementType = property.GetObject().GetType().FindGenericArgumentsOf(typeof(ArrayBuilder<>))[0];
            elementType = TypeCache.GetTypesDerivedFrom(typeof(Builder<>).MakeGenericType(elementType)).Single();
            for (var i = 0; i < valueProperty.arraySize; i++)
            {
                var elementProperty = valueProperty.GetArrayElementAtIndex(i);
                var element = elementProperty.GetObject();
                if (element == null || element.GetType() != elementType) elementProperty.managedReferenceValue = Activator.CreateInstance(elementType);
            }
            EditorGUI.PropertyField(position, valueProperty, label, includeChildren: true);
            property.serializedObject.ApplyModifiedProperties();
        }

        private SerializedProperty ValueProperty(SerializedProperty property)
        {
            var valuePath = $"{property.propertyPath}.{nameof(ArrayBuilder<int>.Value)}";
            return property.serializedObject.FindProperty(valuePath);
        }
    }
}