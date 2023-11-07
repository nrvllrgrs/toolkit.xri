using UnityEngine;
using UnityEditor;
using ToolkitEngine.XR;

namespace ToolkitEditor.XR
{
    [CustomEditor(typeof(XRButtonInteractable))]
    public class XRButtonInteractableEditor : XRBasePositionInteractableEditor
    {
        #region Fields

        protected SerializedProperty m_allowedAngle;

        #endregion

        #region Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            m_allowedAngle = serializedObject.FindProperty(nameof(m_allowedAngle));
        }

        protected override void DrawLinearAxisProperties()
        {
            EditorGUILayout.PropertyField(m_directionAxis, new GUIContent("Direction"));
            EditorGUILayout.PropertyField(m_allowedAngle);
            EditorGUILayout.PropertyField(m_maxDepth);
            EditorGUILayoutUtility.MinMaxSlider(m_normalizedFlowDepths, 0f, 1f);

            EditorGUILayout.PropertyField(m_tailflowSnap);
            if (!m_tailflowSnap.boolValue)
            {
                EditorGUILayout.PropertyField(m_tailflowSpeed);
            }
        }

        #endregion
    }
}