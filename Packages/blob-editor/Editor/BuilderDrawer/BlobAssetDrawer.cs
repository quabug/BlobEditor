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
            var valueType = fieldInfo.FieldType.GenericTypeArguments[0];
            var builderFactory = valueType.GetBuilderFactory(customBuilder: null);
            property = property.FindPropertyRelative(nameof(BlobAsset<int>.Builder));
            var builder = property.GetObject();
            if (builder == null || builder.GetType() != builderFactory.BuilderType)
            {
                property.managedReferenceValue = builderFactory.Create();
                property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }

            EditorGUI.PropertyField(position, property, new GUIContent($"{label.text} : {valueType.ToReadableName()}"), true);

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}