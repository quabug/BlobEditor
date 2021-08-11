using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Blob.Editor
{
    [CustomPropertyDrawer(typeof(PtrBuilder<>), useForChildren: true)]
    public class PtrBuilderDrawer : PropertyDrawer
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
            var valueType = property.GetObject().GetType().FindGenericArgumentsOf(typeof(PtrBuilder<>))[0];
            var builderType = TypeCache.GetTypesDerivedFrom(typeof(Builder<>).MakeGenericType(valueType)).Single();
            var value = valueProperty.GetObject();
            if (value == null || value.GetType() != builderType) valueProperty.managedReferenceValue = Activator.CreateInstance(builderType);
            EditorGUI.PropertyField(position, valueProperty, label, includeChildren: true);
            property.serializedObject.ApplyModifiedProperties();
        }

        private SerializedProperty ValueProperty(SerializedProperty property)
        {
            var valuePath = $"{property.propertyPath}.{nameof(PtrBuilder<int>.Value)}";
            return property.serializedObject.FindProperty(valuePath);
        }
    }
}