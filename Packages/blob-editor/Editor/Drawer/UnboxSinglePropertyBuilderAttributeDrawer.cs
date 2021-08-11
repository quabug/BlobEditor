using UnityEditor;
using UnityEngine;

namespace Blob.Editor
{
    [CustomMultiPropertyDrawer(typeof(UnboxSinglePropertyBuilderAttribute))]
    public class UnboxSinglePropertyBuilderAttributeDrawer : BaseMultiPropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            property = property.FindProperBuilderProperty();
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property = property.FindProperBuilderProperty();
            base.OnGUI(position, property, label);
        }
    }
}