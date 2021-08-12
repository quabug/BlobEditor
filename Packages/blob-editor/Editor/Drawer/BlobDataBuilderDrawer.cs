using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Blob.Editor
{
    [CustomPropertyDrawer(typeof(BlobDataBuilder<>), useForChildren: true)]
    public class BlobDataBuilderDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight;
            if (property.isExpanded)
            {
                var builders = Builders(property);
                for (var i = 0; i < builders.arraySize; i++)
                {
                    var builderProperty = builders.GetArrayElementAtIndex(i);
                    height += EditorGUI.GetPropertyHeight(builderProperty, includeChildren: true);
                }
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();

            var fieldType = fieldInfo == null ? property.GetObject().GetType() : fieldInfo.FieldType;
            var blobType = fieldType.FindGenericArgumentsOf(typeof(Builder<>))[0];
            var blobFields = blobType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var buildersProperty = Builders(property);
            buildersProperty.arraySize = blobFields.Length;
            var builders = (IBuilder[]) buildersProperty.GetObject();
            Array.Resize(ref builders, blobFields.Length);

            var fieldNamesProperty = FieldNames(property);
            fieldNamesProperty.arraySize = blobFields.Length;
            var fieldNames = (string[]) fieldNamesProperty.GetObject();
            Array.Resize(ref fieldNames, blobFields.Length);

            property.serializedObject.ApplyModifiedProperties();

            for (var i = 0; i < blobFields.Length; i++)
            {
                var blobField = blobFields[i];
                var builderType = blobField.FindBuilderType();
                var builder = builders[i];
                if (builder == null || builder.GetType() != builderType || blobField.Name != fieldNames[i])
                {
                    fieldNamesProperty.GetArrayElementAtIndex(i).stringValue = blobField.Name;
                    var builderIndex = Array.IndexOf(fieldNames, blobField.Name);
                    object newBuilder = null;
                    if (builderIndex >= 0) newBuilder = builders[builderIndex];
                    else if (builder != null && builder.GetType() == builderType) newBuilder = builder;
                    else newBuilder = Activator.CreateInstance(builderType);
                    buildersProperty.GetArrayElementAtIndex(i).managedReferenceValue = newBuilder;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }

            position = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(position, property, label);
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                position = EditorGUI.IndentedRect(position);
                var propertyHeight = position.height;
                for (var i = 0; i < blobFields.Length; i++)
                {
                    position = new Rect(position.x, position.y + propertyHeight, position.width, position.height);
                    var builderProperty = buildersProperty.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(position, builderProperty, new GUIContent(blobFields[i].Name), includeChildren: true);
                    propertyHeight = EditorGUI.GetPropertyHeight(builderProperty, includeChildren: true);
                    // HACK (bug?): somehow, `ApplyModifiedProperties` must be place inside `for` loop, otherwise some properties will not be changed in some cases.
                    property.serializedObject.ApplyModifiedProperties();
                }
                EditorGUI.indentLevel--;
            }
            property.serializedObject.ApplyModifiedProperties();
        }

        private SerializedProperty Builders(SerializedProperty property)
        {
            var buildersPath = $"{property.propertyPath}.{nameof(BlobDataBuilder<int>.Builders)}";
            return property.serializedObject.FindProperty(buildersPath);
        }

        private SerializedProperty FieldNames(SerializedProperty property)
        {
            var fieldNamesPath = $"{property.propertyPath}.{nameof(BlobDataBuilder<int>.FieldNames)}";
            return property.serializedObject.FindProperty(fieldNamesPath);
        }
    }
}