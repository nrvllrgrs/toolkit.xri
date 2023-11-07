using UnityEngine;
using UnityEditor;
using ToolkitEngine.XR;

namespace ToolkitEditor.XR
{
    [CustomPropertyDrawer(typeof(XRHandednessEvents.HandednessAction))]
    public class XRHandednessActionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (EditorGUIRectLayout.Foldout(ref position, property, label))
            {
                ++EditorGUI.indentLevel;

                EditorGUIRectLayout.PropertyField(ref position, property.FindPropertyRelative("m_objects"));
                EditorGUIRectLayout.Space(ref position);

                var onGrabbedProp = property.FindPropertyRelative("m_onGrabbed");
                if (EditorGUIRectLayout.Foldout(ref position, onGrabbedProp, "Events"))
                {
                    EditorGUIRectLayout.PropertyField(ref position, onGrabbedProp);
                    EditorGUIRectLayout.PropertyField(ref position, property.FindPropertyRelative("m_onUngrabbed"));
                }

                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight
                + EditorGUIUtility.standardVerticalSpacing;

            if (property.isExpanded)
            {
                var onGrabbedProp = property.FindPropertyRelative("m_onGrabbed");
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_objects"))
                    + EditorGUIRectLayout.GetFoldoutHeight(onGrabbedProp)
                    + (EditorGUIUtility.standardVerticalSpacing * 3f);

                if (onGrabbedProp.isExpanded)
                {
                    height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_onUngrabbed"))
                        + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return height;
        }
    }
}