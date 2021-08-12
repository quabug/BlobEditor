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
            return EditorGUI.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property = property.FindPropertyRelative(nameof(BlobAsset<int>.Builder));
            EditorGUI.PropertyField(position, property, new GUIContent($"{label.text} : {fieldInfo.FieldType.GenericTypeArguments[0].Name}"));
        }
    }
}