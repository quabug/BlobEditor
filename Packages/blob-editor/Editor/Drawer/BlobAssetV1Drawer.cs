using System;
using UnityEditor;
using UnityEngine;

namespace Blob.Editor
{
    [CustomPropertyDrawer(typeof(BlobAssetV1<>)), Obsolete("for backward compatibility of version 1 only")]
    public class BlobAssetV1Drawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            property = property.FindPropertyRelative(nameof(BlobAsset<int>.Builder));
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property = property.FindPropertyRelative(nameof(BlobAsset<int>.Builder));
            EditorGUI.PropertyField(position, property, new GUIContent($"{label.text} : {fieldInfo.FieldType.GenericTypeArguments[0].Name}"));
        }
    }
}