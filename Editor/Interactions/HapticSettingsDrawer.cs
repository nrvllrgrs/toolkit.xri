using UnityEngine;
using UnityEditor;
using ToolkitEngine.XR;

namespace ToolkitEditor.XR
{
    [CustomPropertyDrawer(typeof(HapticSettings))]
    public class HapticSettingsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var intensityProp = property.FindPropertyRelative("m_intensity");
            var durationProp = property.FindPropertyRelative("m_duration");
            var continuousProp = property.FindPropertyRelative("m_continuous");

            EditorGUIRectLayout.PropertyField(ref position, intensityProp);
			EditorGUIRectLayout.PropertyField(ref position, durationProp);
			EditorGUIRectLayout.PropertyField(ref position, continuousProp);

            if (continuousProp.boolValue)
            {
                var delayProp = property.FindPropertyRelative("m_delay");

                ++EditorGUI.indentLevel;
				EditorGUIRectLayout.PropertyField(ref position, delayProp);
                --EditorGUI.indentLevel;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var intensityProp = property.FindPropertyRelative("m_intensity");
            var durationProp = property.FindPropertyRelative("m_duration");
            var continuousProp = property.FindPropertyRelative("m_continuous");

            float height = base.GetPropertyHeight(intensityProp, label)
                + base.GetPropertyHeight(durationProp, label)
                + base.GetPropertyHeight(continuousProp, label)
                + (EditorGUIUtility.standardVerticalSpacing * 2f);

            if (continuousProp.boolValue)
            {
                var delayProp = property.FindPropertyRelative("m_delay");
                height += base.GetPropertyHeight(delayProp, label)
                    + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }
    }
}