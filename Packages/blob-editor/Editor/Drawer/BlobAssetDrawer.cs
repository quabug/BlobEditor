using System;
using UnityEditor;
using UnityEngine;

namespace Blob.Editor
{
    [CustomPropertyDrawer(typeof(BlobAsset<>))]
    public class BlobAssetDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            property = property.FindPropertyRelative(nameof(BlobAsset<int>.Builder));
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();

            var valueType = fieldInfo.FieldType.GenericTypeArguments[0];
            var builderType = valueType.FindBuilderType(customBuilder: null);
            property = property.FindPropertyRelative(nameof(BlobAsset<int>.Builder));
            var builder = property.GetObject();
            if (builder == null || builder.GetType() != builderType)
            {
                property.managedReferenceValue = Activator.CreateInstance(builderType);
                property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }

            EditorGUI.PropertyField(position, property, new GUIContent($"{label.text} : {valueType.ToReadableName()}"), true);

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}